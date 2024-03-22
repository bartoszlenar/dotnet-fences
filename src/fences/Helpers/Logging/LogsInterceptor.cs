namespace Fences.Helpers.Logging;

using Fences.Commands;
using Spectre.Console.Cli;

public class LogsInterceptor : ICommandInterceptor
{
    public static bool IsLoggingEnabled { get; private set; }

    public void Intercept(CommandContext context, CommandSettings settings)
    {
        if (settings is GlobalSettings logSettings)
        {
            IsLoggingEnabled = logSettings.IsLoggingEnabled;
        }
    }
}
