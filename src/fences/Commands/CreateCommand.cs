namespace fences.Commands;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

sealed class CreateCommand : AsyncCommand<CreateCommand.Settings>
{
    public sealed class Settings : GlobalSettings
    {

        [CommandArgument(0, "[path]")]
        [DefaultValue(".")]
        public string? Path { get; set; }
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.Write("Will create fence in path: " + Path.Combine(Environment.CurrentDirectory, settings.Path));

        return Task.FromResult(0);
    }
}
