namespace fences.Helpers.AssemblyAttributes;

using System.Globalization;

[AttributeUsage(AttributeTargets.Assembly)]
internal class BuildTimeAttribute : Attribute
{
    public BuildTimeAttribute(string value)
    {
        BuildTime = DateTimeOffset.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    public DateTimeOffset BuildTime { get; }
}