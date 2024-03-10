using fences.Helpers;
using fences.Helpers.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace fences;

class App
{
    private static IServiceCollection GetDefaultServiceCollection()
    {
        var appInfo = AppInfo.FromAssembly(typeof(App).Assembly);

        var services = new ServiceCollection();

        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<App>());
        services.AddSingleton(appInfo);

        return services;
    }

    public static App Create(Func<IServiceCollection, IServiceCollection>? configureServices = null)
    {
        var services = GetDefaultServiceCollection();

        if (configureServices != null)
        {
            services = configureServices(services);
        }

        return new App(services.BuildServiceProvider());
    }

    private App(ServiceProvider serviceProvider)
    {
        _commandApp = new CommandApp();

        _commandApp.Configure(config =>
        {
            var appInfo = serviceProvider.GetRequiredService<AppInfo>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            config.SetApplicationName(appInfo.Name);
            config.SetApplicationVersion(appInfo.Version);

            config.AddFeature<PrintAboutRequest>(mediator, "about")
                .WithDescription("Prints detailed info about the application.");
        });
    }

    private CommandApp _commandApp;

    public Task<int> Run(IEnumerable<string> args)
    {
        return _commandApp.RunAsync(args);
    }
}