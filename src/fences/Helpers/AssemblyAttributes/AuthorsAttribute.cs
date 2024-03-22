namespace Fences.Helpers.AssemblyAttributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class AuthorsAttribute(string author) : Attribute
{
    public string Author { get; } = !string.IsNullOrWhiteSpace(author) ? author : "(unknown)";
}
