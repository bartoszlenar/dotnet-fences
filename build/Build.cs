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

class Build : NukeBuild
{

    static readonly DateTimeOffset BuildTime = DateTimeOffset.UtcNow;

    public static int Main() => Execute<Build>(x => x.CompileForPack);

    AbsolutePath SolutionFilePath => RootDirectory / "fences.sln";
    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath FenceProjectFilePath => SourceDirectory / "fences" / "fences.csproj";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath NuGetArtifactsDirectory => ArtifactsDirectory / Version / "nuget";

    AbsolutePath FenceTestProjectFilePath => SourceDirectory / "fences.Tests" / "fences.Tests.csproj";

    [Parameter("Allows warning during the build. Default is false. Set to true to stop the warnings from breaking the build.")]
    readonly bool AllowWarnings;

    [Parameter("Version. If not provided it will be generated automatically as 0.0.0-timestamp")]
    string? Version;

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
            new[] { ArtifactsDirectory, NuGetArtifactsDirectory }.ForEach(dir => dir.DeleteDirectory());
            RootDirectory.GlobDirectories("**/bin", "**/obj").ForEach(dir => dir.DeleteDirectory());
        });

    Target Clean => _ => _
        .Description("Clean all compiled content (bin, obj) of the project.")
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(dir => dir.DeleteDirectory());
        });

    Target CompileForPack => _ => _
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetConfiguration(Configuration.Release)
                .SetProjectFile(FenceProjectFilePath)
                .SetTreatWarningsAsErrors(true));
        })
        .Produces(NuGetArtifactsDirectory / "*.nupkg")
        .Unlisted();

    Target Pack => _ => _
        .Description("Packs the project into a nuget package.")
        .DependsOn(CompileForPack)
        .Executes(() =>
        {
            DotNetPack(s => s
                .EnableNoBuild()
                .SetProject(FenceProjectFilePath)
                .SetOutputDirectory(NuGetArtifactsDirectory)
                .SetVersion(Version)
            );
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
}
