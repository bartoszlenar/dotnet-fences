using System.Reflection;
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

var mediator = services.GetRequiredService<IMediator>();

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationVersion(programInfo.Version);
    config.SetApplicationName(programInfo.Name);

    config.AddFeature<PrintAboutRequest>(mediator, "about")
        .WithDescription("Prints detailed info about the application.");
});

return await app.RunAsync(args);

