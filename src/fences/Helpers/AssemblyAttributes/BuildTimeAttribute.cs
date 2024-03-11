#pragma warning disable CA1019 // Define accessors for attribute arguments

namespace fences.Helpers.AssemblyAttributes;

using System.Globalization;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class BuildTimeAttribute : Attribute
{
    public BuildTimeAttribute(string buildTime)
    {
        BuildTime = DateTimeOffset.ParseExact(buildTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    public DateTimeOffset BuildTime { get; }
}