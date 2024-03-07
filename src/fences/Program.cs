using System.Globalization;
using System.Reflection;
using fences;
using Spectre.Console.Cli;

var programInfo = AppInfo.FromAssembly(Assembly.GetExecutingAssembly());

var app = new CommandApp<InfoCommand>();

app.Configure(config =>
{
    config.SetApplicationVersion(programInfo.Version);
    config.SetApplicationName(programInfo.Name);
});

return await app.RunAsync(args);
