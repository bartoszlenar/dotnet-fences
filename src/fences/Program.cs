using System.Reflection;
using fences;
using Spectre.Console.Cli;

var app = new CommandApp<InfoCommand>();

app.Configure(config =>
{
    config.SetApplicationVersion(typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion!);
});

return await app.RunAsync(args);
