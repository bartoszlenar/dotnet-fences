using System.Reflection;

record AppInfo(string Name, string FullVersion, string Version, string Commit, DateTimeOffset BuildTime)
{
    public static AppInfo FromAssembly(Assembly assembly)
    {
        var fullVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        var name = assembly.GetName().Name!;
        var semVer = fullVersion.Split('+')[0];
        var commit = fullVersion.Split('+')[1];
        var buildTime = assembly.GetCustomAttribute<BuildTimeAttribute>()!.BuildTime;

        return new AppInfo(name, fullVersion, semVer, commit, buildTime);
    }
}