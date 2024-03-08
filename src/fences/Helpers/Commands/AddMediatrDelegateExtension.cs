using MediatR;
using Spectre.Console.Cli;

namespace fences.Helpers.Commands;

public static class AddMediatrFeatureExtensions
{
    public static ICommandConfigurator AddMediatrFeature<TCommand>(this IConfigurator<CommandSettings> configurator, IMediator mediator, string name) where TCommand : CommandSettings, IRequest<int>
    {
        return configurator.AddAsyncDelegate<TCommand>(name, (context, request) =>
        {
            return mediator.Send(request);
        });
    }
}
