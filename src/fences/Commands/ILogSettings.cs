namespace fences.Commands;

using System.ComponentModel;
using Spectre.Console.Cli;

internal interface ILogSettings
{
    public bool? IsLoggingEnabled { get; set; }
}
