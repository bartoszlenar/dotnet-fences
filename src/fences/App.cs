namespace fences;

using fences.Commands;
using fences.Helpers;
using fences.Helpers.Infrastructure;
using fences.Injection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Spectre;
using Spectre.Console;
using Spectre.Console.Cli;
using ILogger = Microsoft.Extensions.Logging.ILogger;

class App
{

    static AppInfo appInfo = AppInfo.FromAssembly(typeof(App).Assembly);

    private static ServiceCollection GetDefaultServiceCollection()
    {

        var services = new ServiceCollection();

        services.AddSingleton(appInfo);

        services.AddLogging(builder =>
        {
            builder.AddSerilog(new LoggerConfiguration()
                .WriteTo.Spectre()
                .CreateLogger());
        });

        return services;
    }

    public static App Create(Func<ServiceCollection, ServiceCollection>? configureServices = null)
    {
        var services = GetDefaultServiceCollection();

        if (configureServices != null)
        {
            services = configureServices(services);
        }

        return new App(services);
    }

    private App(ServiceCollection serviceCollection)
    {
        var typeRegistrar = new TypeRegistrar(serviceCollection);

        _commandApp = new CommandApp<CheckCommand>(typeRegistrar);

        _commandApp.Configure(config =>
        {
            config.SetApplicationName(appInfo.Name);
            config.SetApplicationVersion(appInfo.Version);
            config.SetInterceptor(new LogInterceptor());
            config.AddCommand<AboutCommand>("about");
            config.AddCommand<CheckCommand>("check");
        });

    }

    private ICommandApp _commandApp;

    public Task<int> Run(IEnumerable<string> args)
    {
        return _commandApp.RunAsync(args);
    }
}
