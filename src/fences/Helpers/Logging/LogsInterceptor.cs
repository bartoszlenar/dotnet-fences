using fences.Commands;
using Serilog.Core;
using Spectre.Console;
using Spectre.Console.Cli;

namespace fences.Helpers.Infrastructure;

public class LogInterceptor : ICommandInterceptor
{
    public static bool IsLoggingEnabled { get; private set; }

    public void Intercept(CommandContext context, CommandSettings settings)
    {
        AnsiConsole.Write("Intercepting command: " + settings.ToString());
        if (settings is GlobalSettings logSettings)
        {
            AnsiConsole.Write("Intercepting logsettings: " + logSettings.IsLoggingEnabled);
            IsLoggingEnabled = logSettings.IsLoggingEnabled ?? false;
        }
    }
}
