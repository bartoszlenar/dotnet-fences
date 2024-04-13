namespace Fences.Injection;

using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

public sealed class TypeRegistrar(IServiceCollection builder) : ITypeRegistrar
{
    private readonly IServiceCollection _builder = builder;

    public ITypeResolver Build() => new TypeResolver(_builder.BuildServiceProvider());

    public void Register(Type service, Type implementation) =>
        _builder.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation) => _builder.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        _builder.AddSingleton(service, (_) => factory());
    }
}
