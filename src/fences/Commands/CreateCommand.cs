namespace Fences.Commands;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class CreateCommand : AsyncCommand<CreateCommand.Settings>
{
    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.Write("Creating fence: " + Path.Combine(Environment.CurrentDirectory, settings.Path));

        return Task.FromResult(0);
    }

    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "[path]")]
        [DefaultValue(".")]
        public string Path { get; init; } = ".";
    }
}
