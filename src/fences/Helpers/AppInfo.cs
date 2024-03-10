namespace fences.Helpers;

using System.Reflection;
using fences.Helpers.AssemblyAttributes;

record AppInfo(string Name, string FullVersion, string Version, string Commit, DateTimeOffset BuildTime, string ProjectUrl, string Author)
{
    public static AppInfo FromAssembly(Assembly assembly)
    {
        var fullVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        var name = assembly.GetName().Name!;
        var semVer = fullVersion.Split('+')[0];
        var commit = fullVersion.Split('+')[1];
        var buildTime = assembly.GetCustomAttribute<BuildTimeAttribute>()!.BuildTime;
        var projectUrl = assembly.GetCustomAttribute<ProjectUrlAttribute>()!.ProjectUrl;
        var author = assembly.GetCustomAttribute<AuthorsAttribute>()!.Author;

        return new AppInfo(name, fullVersion, semVer, commit, buildTime, projectUrl, author);
    }
}