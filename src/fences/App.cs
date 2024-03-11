namespace fences;

using fences.Features;
using fences.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

class App
{
    private static ServiceCollection GetDefaultServiceCollection()
    {
        var appInfo = AppInfo.FromAssembly(typeof(App).Assembly);

        var services = new ServiceCollection();

        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<App>());
        services.AddSingleton(appInfo);

        return services;
    }

    public static App Create(Func<ServiceCollection, ServiceCollection>? configureServices = null)
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

            config.AddPrintAboutFeature("about", mediator);
            config.AddRunFeature("run", mediator);
        });
    }

    private CommandApp _commandApp;

    public Task<int> Run(IEnumerable<string> args)
    {
        return _commandApp.RunAsync(args);
    }
}