using MediatR;
using Spectre.Console.Cli;

namespace fences;

public static class AddMediatrDelegateExtension
{
    public static ICommandConfigurator AddMediatrFeature<TCommand>(this IConfigurator<CommandSettings> configurator, IMediator mediator, string name) where TCommand : CommandSettings, IRequest<int>
    {
        return configurator.AddAsyncDelegate<TCommand>(name, (context, request) =>
        {
            return mediator.Send(request);
        });
    }
}
