namespace fences.Helpers.AssemblyAttributes;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class AuthorsAttribute : Attribute
{
    public AuthorsAttribute(string author)
    {
        Author = !string.IsNullOrWhiteSpace(author) ? author : "(unknown)";
    }

    public string Author { get; }
}