using Fences;
using Fences.Helpers.Debugging;
using Spectre.Console;

if (DebuggerAwaiter.ShouldWaitForDebugger(args))
{
    if (!DebuggerAwaiter.IsDebugBuild)
    {
        AnsiConsole.Markup("[red]Cannot wait for debugger on a non-debug build.[/]");
        return -10;
    }

    await DebuggerAwaiter.WaitForDebugger();
}

var app = App.Create();

return await app.Run(args).ConfigureAwait(false);
