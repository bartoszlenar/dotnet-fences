namespace fences.Commands;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

sealed class CheckCommand : AsyncCommand<CheckCommand.Settings>
{
    private readonly ILogger _logger;

    public CheckCommand(ILogger logger)
    {
        _logger = logger;
    }

    public sealed class Settings : GlobalSettings
    {

        [CommandArgument(0, "[path]")]
        [DefaultValue(".")]
        public string? Path { get; set; }
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.Write("Will check path: " + Path.Combine(Environment.CurrentDirectory, settings.Path));

        _logger.LogInformation("LOGGING!!!");

        return Task.FromResult(0);
    }
}
