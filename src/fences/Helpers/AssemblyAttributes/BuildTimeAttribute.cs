#pragma warning disable CA1019 // Define accessors for attribute arguments

namespace Fences.Helpers.AssemblyAttributes;

using System.Globalization;

[AttributeUsage(AttributeTargets.Assembly)]
internal sealed class BuildTimeAttribute(string buildTime) : Attribute
{
    public DateTimeOffset BuildTime { get; } = DateTimeOffset.ParseExact(buildTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
}
