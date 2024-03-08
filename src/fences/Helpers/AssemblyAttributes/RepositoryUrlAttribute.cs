namespace fences.Helpers.AssemblyAttributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal class RepositoryUrlAttribute : Attribute
{
    public RepositoryUrlAttribute(string value)
    {
        RepositoryUrl = value ?? "(unknown)";
    }

    public string RepositoryUrl { get; }
}