namespace fences.Helpers.AssemblyAttributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal class ProjectUrlAttribute : Attribute
{
    public ProjectUrlAttribute(string value)
    {
        ProjectUrl = !string.IsNullOrWhiteSpace(value) ? value : "(unknown)";
    }

    public string ProjectUrl { get; }
}