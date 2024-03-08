using System.Globalization;
using System.Reflection;
using fences;
using fences.Helpers;
using fences.Helpers.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;



var programInfo = AppInfo.FromAssembly(Assembly.GetExecutingAssembly());

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(programInfo);
serviceCollection.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

var services = serviceCollection.BuildServiceProvider();

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationVersion(programInfo.Version);
    config.SetApplicationName(programInfo.Name);

    config.AddBranch("show", b =>
    {
        b.AddMediatrFeature<PrintInfoRequest>(services.GetRequiredService<IMediator>(), "info")
            .WithDescription("Prints information about the application.");
    });
});

return await app.RunAsync(args);

