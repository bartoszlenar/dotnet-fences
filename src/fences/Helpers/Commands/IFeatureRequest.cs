namespace fences.Helpers.Commands;

using MediatR;
using Spectre.Console.Cli;

public abstract class FeatureRequest : CommandSettings, IRequest<int>
{
}