namespace fences.Helpers.AssemblyAttributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal class AuthorsAttribute : Attribute
{
    public AuthorsAttribute(string value)
    {
        Author = !string.IsNullOrWhiteSpace(value) ? value : "(unknown)";
    }

    public string Author { get; }
}