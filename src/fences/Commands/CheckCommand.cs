namespace fences.Commands;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

sealed class CheckCommand : AsyncCommand<CheckCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {

        [CommandArgument(0, "[path]")]
        [DefaultValue(".")]
        public string? Path { get; set; }
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.Write("Will check path: " + Path.Combine(Environment.CurrentDirectory, settings.Path));

        return Task.FromResult(0);
    }
}
