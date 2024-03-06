using System.Reflection;

record ProgramInfo(string Name, string FullVersion, string Version, string Commit)
{
    public static ProgramInfo FromAssembly(Assembly assembly)
    {
        var fullVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        var name = assembly.GetName().Name!;
        var semVer = fullVersion.Split('+')[0];
        var commit = fullVersion.Split('+')[1];

        return new ProgramInfo(name, fullVersion, semVer, commit);
    }
}