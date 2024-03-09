namespace fences.Helpers.Commands;

using MediatR;

interface IFeatureHandler : IRequestHandler<PrintAboutRequest, int> { }