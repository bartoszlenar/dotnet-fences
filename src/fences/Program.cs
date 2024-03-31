using Fences;
using Fences.Helpers.Debugging;

if (DebuggerAwaiter.ShouldWaitForDebugger(args))
{
    await DebuggerAwaiter.WaitForDebugger();
}

var app = App.Create();

return await app.Run(args).ConfigureAwait(false);
