namespace Fences.Commands;

using System.ComponentModel;
using Spectre.Console.Cli;

public class GlobalSettings : CommandSettings, ILogSettings
{
    [CommandOption("-l|--logs")]
    [Description("Enable detailed logging")]
    [DefaultValue(false)]
    public bool IsLoggingEnabled { get; set; }
}
