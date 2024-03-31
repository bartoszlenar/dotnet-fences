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
        if (!IsDebugBuild)
        {
            AnsiConsole.MarkupLine(CultureInfo.InvariantCulture, "[red]This is NOT a debug build, so your debugging experience might be poor (if it triggers...).[/]");
            return;
        }

        AnsiConsole.Markup(CultureInfo.InvariantCulture, "[yellow]Waiting for debugger to attach to process [bold]{0}[/][/]", Environment.ProcessId);

        while (!Debugger.IsAttached)
        {
            AnsiConsole.Markup("[yellow].[/]");
            await Task.Delay(2000);
        }
    }
}
