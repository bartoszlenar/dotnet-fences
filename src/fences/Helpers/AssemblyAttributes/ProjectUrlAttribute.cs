namespace fences.Helpers.AssemblyAttributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class ProjectUrlAttribute : Attribute
{
    public ProjectUrlAttribute(string projectUrl)
    {
        ProjectUrl = !string.IsNullOrWhiteSpace(projectUrl) ? projectUrl : "(unknown)";
    }

    public string ProjectUrl { get; }
}