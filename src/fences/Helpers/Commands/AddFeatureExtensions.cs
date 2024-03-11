using MediatR;
using Spectre.Console.Cli;

namespace fences.Helpers.Commands;

static class AddFeatureExtensions
{
    public static ICommandConfigurator AddFeature<TCommand>(this IConfigurator configurator, IMediator mediator, string name) where TCommand : CommandSettings, IRequest<int>
    {
        return configurator.AddAsyncDelegate<TCommand>(name, (context, request) =>
        {
            return mediator.Send(request);
        });
    }

    public static ICommandConfigurator AddFeature<TCommand>(this IConfigurator<CommandSettings> configurator, IMediator mediator, string name) where TCommand : CommandSettings, IRequest<int>
    {
        return configurator.AddAsyncDelegate<TCommand>(name, (context, request) =>
        {
            return mediator.Send(request);
        });
    }
}
