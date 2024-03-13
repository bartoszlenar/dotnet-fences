namespace fences.Features;

using fences.Helpers;
using fences.Helpers.Commands;
using MediatR;
using Spectre.Console;
using Spectre.Console.Cli;

public sealed class RunRequest : FeatureRequest
{
    [CommandArgument(0, "<path>")]
    public string? Path { get; set; }
}

class RunFeature : IFeatureHandler<RunRequest>
{
    private readonly AppInfo _appInfo;

    public RunFeature(AppInfo appInfo)
    {
        _appInfo = appInfo;
    }

    public Task<int> Handle(RunRequest request, CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine("Run in path: " + request.Path);

        var fullPath = Path.GetFullPath(request.Path ?? ".", Environment.CurrentDirectory);

        AnsiConsole.WriteLine("Full path: " + fullPath);

        return Task.FromResult(0);
    }
}

static class RunFeatureExtensions
{
    public static ICommandConfigurator AddRunFeature(this IConfigurator configurator, string name, IMediator mediator) => configurator
            .AddFeature<RunRequest>(mediator, "run")
            .WithDescription("Run the processing in the specified path.");
}
