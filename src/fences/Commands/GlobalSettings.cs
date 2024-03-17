namespace fences.Commands;

using System.ComponentModel;
using Spectre.Console.Cli;

public class GlobalSettings : CommandSettings, ILogSettings
{

    [CommandOption("-l|--logs")]
    [Description("Enable logging to console output")]
    [DefaultValue(false)]
    public bool? IsLoggingEnabled { get; set; }
}
