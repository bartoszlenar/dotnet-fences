namespace fences.Commands;

using System.ComponentModel;
using Spectre.Console.Cli;

public class GlobalSettings : CommandSettings
{

    [CommandOption("-l|--logs")]
    [Description("Logs level")]
    [DefaultValue("none")]
    public string? Logs { get; set; }
}
