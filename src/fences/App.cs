namespace Fences;

using Fences.Commands;
using Fences.Helpers;
using Fences.Helpers.Logging;
using Fences.Injection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Spectre;
using Spectre.Console.Cli;

public sealed class App
{
    private static readonly AppInfo AppInfo = AppInfo.FromAssembly(typeof(App).Assembly);

    private static readonly LogsInterceptor LogsInterceptor = new();

    private readonly ICommandApp commandApp;

    private App(ServiceCollection serviceCollection)
    {
        var typeRegistrar = new TypeRegistrar(serviceCollection);

        this.commandApp = new CommandApp<RunCommand>(typeRegistrar);

        this.commandApp.Configure(config =>
        {
            config.SetApplicationName(AppInfo.Name);
            config.SetApplicationVersion(AppInfo.Version);
            config.SetInterceptor(LogsInterceptor);
            config.AddCommand<AboutCommand>("about");
            config.AddCommand<CheckCommand>("check");
            config.AddCommand<RunCommand>("run");
            config.AddCommand<CreateCommand>("create");
        });
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

    public Task<int> Run(IEnumerable<string> args) => this.commandApp.RunAsync(args);

    private static ServiceCollection GetDefaultServiceCollection()
    {
        var services = new ServiceCollection();

        services.AddSingleton(AppInfo);

        services.AddLogging(builder => builder.AddSerilog(new LoggerConfiguration()
                    .Filter.ByExcluding(_ => !LogsInterceptor.IsLoggingEnabled)
                    .MinimumLevel.Verbose()
                    .WriteTo.Spectre()
                    .CreateLogger()));

        return services;
    }
}
