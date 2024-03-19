namespace fences;

using fences.Commands;
using fences.Helpers;
using fences.Helpers.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Spectre;
using Spectre.Console;
using Spectre.Console.Cli;

class App
{
    private static ServiceCollection GetDefaultServiceCollection()
    {
        var appInfo = AppInfo.FromAssembly(typeof(App).Assembly);

        var services = new ServiceCollection();

        services.AddSingleton(appInfo);

        services.AddLogging(c => c
            .AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Spectre()
                .CreateLogger()));

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
        _commandApp = new CommandApp<CheckCommand>();

        _commandApp.Configure(config =>
        {
            var appInfo = serviceProvider.GetRequiredService<AppInfo>();

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
