namespace Fences.Commands;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class RunCommand(ILogger<RunCommand> logger) : AsyncCommand<RunCommand.Settings>
{
    private readonly ILogger<RunCommand> logger = logger;

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        this.logger.LogInformation("Current directory: {CurrentDirectory}", Environment.CurrentDirectory);
        this.logger.LogInformation("Path: {Path}", settings.Path);

        AnsiConsole.Write("Running at path: " + Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, settings.Path)));

        return Task.FromResult(0);
    }

    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "[path]")]
        [DefaultValue(".")]
        public string Path { get; init; } = ".";
    }
}
