namespace Fences.Helpers.Debugging;

using System.Diagnostics;
using System.Globalization;
using Spectre.Console;

internal static class DebuggerAwaiter
{
    public static string WaitForDebuggerFlag => "--debug";

    public static bool ShouldWaitForDebugger(IReadOnlyCollection<string> args) => args.Contains(WaitForDebuggerFlag, StringComparer.OrdinalIgnoreCase);

    public static bool IsDebugBuild
    {
        get
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }

    public static async Task WaitForDebugger()
    {
        AnsiConsole.Markup(CultureInfo.InvariantCulture, "[yellow]Waiting for debugger to attach to process [bold]{0}[/][/]", Environment.ProcessId);

        var i = 0;
        var dotOnEveryStep = 4;
        var stepInMilliseconds = 500;

        while (!Debugger.IsAttached)
        {
            if (i++ % dotOnEveryStep == 0)
            {
                AnsiConsole.Markup("[yellow].[/]");
            }

            await Task.Delay(stepInMilliseconds);
        }
    }
}
