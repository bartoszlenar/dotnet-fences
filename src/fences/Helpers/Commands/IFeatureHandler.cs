namespace fences.Helpers.Commands;

using MediatR;

interface IFeatureHandler<T> : IRequestHandler<T, int> where T : FeatureRequest { }