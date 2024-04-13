namespace Fences.Helpers.Debugging;

using System.Diagnostics;
using System.Globalization;
using Spectre.Console;

internal static class DebuggerAwaiter
{
    public static string WaitForDebuggerFlag => "--debug";

#pragma warning disable IDE0025 // Use expression body for property
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
#pragma warning restore IDE0025 // Use expression body for property

    public static bool ShouldWaitForDebugger(IReadOnlyCollection<string> args) => args.Contains(WaitForDebuggerFlag, StringComparer.OrdinalIgnoreCase);

    public static async Task WaitForDebugger()
    {
        AnsiConsole.Markup(CultureInfo.InvariantCulture, "[yellow]Waiting for debugger to attach to process [bold]{0}[/][/]", Environment.ProcessId);

        var i = 0;
        const int dotOnEveryStep = 4;
        const int stepInMilliseconds = 500;

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
