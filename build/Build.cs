using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using System.Text.RegularExpressions;
using System.Globalization;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Utilities;
using System.IO;

class Build : NukeBuild
{
    static readonly DateTimeOffset BuildTime = DateTimeOffset.UtcNow;

    public static int Main() => Execute<Build>(x => x.CompileForRelease);

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath FenceProjectFilePath => SourceDirectory / "fences" / "fences.csproj";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath BuildArtifactsDirectory => ArtifactsDirectory / Version;
    AbsolutePath TestArtifactsDirectory => BuildArtifactsDirectory / "tests";
    AbsolutePath ToolsDirectory => RootDirectory / "tools";
    AbsolutePath FenceTestProjectFilePath => SourceDirectory / "fences.Tests" / "fences.Tests.csproj";

    [Parameter("Allows warning during the build. Default is false. Set to true to stop the warnings from breaking the build.")]
    readonly bool AllowWarnings;

    [Parameter("Version. If not provided it will be generated automatically as 0.0.0-timestamp")]
    string? Version;

    [Parameter($"Some targets try to show you their results just after they finish (like opening the coverage report in the browser). Set to true to enable this feature. Default is false.")]
    readonly bool OpenResults = false;

    [Parameter("Commit SHA")]
    readonly string? CommitSha;

    protected override void OnBuildInitialized()
    {
        base.OnBuildCreated();

        Version = ResolveVersion(Version);

        Serilog.Log.Information($"Version: {Version}");
    }

    Target CleanAll => _ => _
        .Description("Clean all generated directories (artifacts, tools, builds, temps, etc.)")
        .Executes(() =>
        {
            new[] { ArtifactsDirectory, BuildArtifactsDirectory, ToolsDirectory }.ForEach(dir => dir.DeleteDirectory());
            RootDirectory.GlobDirectories("**/bin", "**/obj").ForEach(dir => dir.DeleteDirectory());
        });

    Target Clean => _ => _
        .Description("Clean all compiled content (bin, obj) of the project.")
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(dir => dir.DeleteDirectory());
        });

    Target CompileForRelease => _ => _
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetConfiguration(Configuration.Release)
                .SetProjectFile(FenceProjectFilePath)
                .SetTreatWarningsAsErrors(true));
        })
        .Unlisted();

    Target Pack => _ => _
        .Description("Packs the project into a nuget package.")
        .DependsOn(CompileForRelease)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetConfiguration(Configuration.Release)
                .EnableNoBuild()
                .SetProject(FenceProjectFilePath)
                .SetOutputDirectory(BuildArtifactsDirectory)
                .SetVersion(Version)
            );
        });

    Target CompileForDebug => _ => _
       .Executes(() =>
       {
           new[] { FenceProjectFilePath, FenceTestProjectFilePath }.ForEach(path =>
           {
               DotNetBuild(s => s
                   .SetConfiguration(Configuration.Debug)
                   .SetProjectFile(path)
                   .SetTreatWarningsAsErrors(!AllowWarnings));
           });
       })
       .Unlisted();

    Target Test => _ => _
        .DependsOn(CompileForDebug)
        .Executes(() =>
        {
            DotNetTest(p => p
                .EnableNoBuild()
                .SetConfiguration(Configuration.Debug)
                .SetProjectFile(FenceTestProjectFilePath)
                .SetLoggers($"junit;LogFilePath={TestArtifactsDirectory / $"report.junit"}")
                .SetProcessArgumentConfigurator(args => args
                    .Add(@"--collect:""XPlat Code Coverage""")
                    .Add(@$"--results-directory:""{TestArtifactsDirectory}"""))
                );

            var coverageFile = TestArtifactsDirectory.GlobFiles("**/coverage.cobertura.xml").FirstOrDefault();

            if (coverageFile is null)
            {
                Serilog.Log.Warning("No coverage report file found.");
                return;
            }

            Serilog.Log.Information($"Moving coverage file {coverageFile} to the artifacts directory {TestArtifactsDirectory}.");
            coverageFile.MoveToDirectory(TestArtifactsDirectory);
            coverageFile.Parent.DeleteDirectory();

            if (OpenResults)
            {
                TestArtifactsDirectory.Open();
            }
        });

    Target CoverageReport => _ => _
        .Description("Generates the human-readable coverage report and opens it in the browser.")
        .DependsOn(Test)
        .Executes(() =>
        {
            var coverageFile = TestArtifactsDirectory / "coverage.cobertura.xml";

            var coverageReportDirectory = BuildArtifactsDirectory / "tests_report";

            var coverageReportHistoryDirectory = ArtifactsDirectory / "_code_coverage_history";

            var toolParameters = new[]
            {
                $"-reports:{coverageFile}",
                $"-reporttypes:HTML;JsonSummary",
                $"-targetdir:{coverageReportDirectory}",
                $"-historydir:{coverageReportHistoryDirectory}",
                $"-title:dotnet-fences unit tests code coverage report",
                $"-tag:v{Version}" + (CommitSha is null ? "" : $"+{CommitSha}"),
            };

            var tool = GetTool("dotnet-reportgenerator-globaltool", "5.2.2", "ReportGenerator.dll", "net8.0");

            ExecuteTool(tool, toolParameters.Select(p => $"\"{p}\"").JoinSpace());

            if (OpenResults)
            {
                Serilog.Log.Information($"Report generated in {coverageReportDirectory} and will be opened in the browser.");
                (coverageReportDirectory / "index.html").Open();
            }
        });

    string ResolveVersion(string? version)
    {
        if (version is null)
        {
            Serilog.Log.Warning($"Version missing, it will be generated automatically.");
            return $"0.0.0-{BuildTime.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture)}";
        }

        var semVerRegex = new Regex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$", RegexOptions.Compiled);

        if (!semVerRegex.IsMatch(version))
        {
            Serilog.Log.Fatal($"Version is set to: {version} which is not a valid semver value. Please provide a valid semver value or leave it empty to generate it automatically.");
            throw new Exception($"Version {version} is not a valid semver value");
        }

        return version;
    }

    AbsolutePath GetTool(string name, string version, string executableFileName, string? framework = null)
    {
        var frameworkPart = framework is null ? $" (framework {framework})" : string.Empty;

        var toolInfo = $"{name} {version}{frameworkPart}, executable file: {executableFileName}";

        Serilog.Log.Information($"Looking for tool: {toolInfo}");

        var toolPath = ResolveToolPath();

        if (toolPath is null)
        {
            DotNetToolInstall(c => c
                    .SetPackageName(name)
                    .SetVersion(version)
                    .SetToolInstallationPath(ToolsDirectory)
                    .SetGlobal(false));
        }

        toolPath = ResolveToolPath();

        if (toolPath is null)
        {
            Serilog.Log.Fatal($"Tool {toolInfo}: not found in the tools directory after intallation. Enable more detailed logging to see the search pattern and more details.");
            throw new Exception($"Could not find tool {name}");
        }

        return toolPath;

        AbsolutePath? ResolveToolPath()
        {
            var frameworkPart = framework != null ? (framework + "/**/") : string.Empty;

            Serilog.Log.Debug($"Looking for tool in {ToolsDirectory} using glob pattern: **/{name}/{version}/**/{frameworkPart}{executableFileName}");

            var files = ToolsDirectory.GlobFiles($"**/{name}/{version}/**/{frameworkPart}{executableFileName}");

            if (files.Count > 1)
            {
                foreach (var file in files)
                {
                    Serilog.Log.Warning($"Found tool candidate: {file}");
                }

                var toolPath = files.First();

                Serilog.Log.Warning($"Found many tool candidates, so proceeding with the first one: {toolPath}");

                return toolPath;
            }

            return files.FirstOrDefault();
        }
    }

    void ExecuteTool(AbsolutePath tool, string parameters)
    {
        ProcessTasks.StartProcess(ToolPathResolver.GetPathExecutable("dotnet"), tool + " -- " + parameters).AssertZeroExitCode();
    }
}
