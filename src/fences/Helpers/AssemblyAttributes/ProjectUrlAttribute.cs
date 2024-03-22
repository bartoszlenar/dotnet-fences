namespace Fences.Helpers.AssemblyAttributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class ProjectUrlAttribute(string projectUrl) : Attribute
{
    public string ProjectUrl { get; } = !string.IsNullOrWhiteSpace(projectUrl) ? projectUrl : "(unknown)";
}
