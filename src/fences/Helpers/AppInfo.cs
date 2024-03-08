namespace fences.Helpers;

using System.Reflection;
using fences.Helpers.AssemblyAttributes;

record AppInfo(string Name, string FullVersion, string Version, string Commit, DateTimeOffset BuildTime, string RepositoryUrl)
{
    public static AppInfo FromAssembly(Assembly assembly)
    {
        var fullVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        var name = assembly.GetName().Name!;
        var semVer = fullVersion.Split('+')[0];
        var commit = fullVersion.Split('+')[1];
        var buildTime = assembly.GetCustomAttribute<BuildTimeAttribute>()!.BuildTime;
        var repositoryUrl = assembly.GetCustomAttribute<RepositoryUrlAttribute>()!.RepositoryUrl;

        return new AppInfo(name, fullVersion, semVer, commit, buildTime, repositoryUrl);
    }
}