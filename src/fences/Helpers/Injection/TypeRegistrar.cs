namespace Fences.Injection;

using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

public sealed class TypeRegistrar(IServiceCollection builder) : ITypeRegistrar
{
    private readonly IServiceCollection builder = builder;

    public ITypeResolver Build() => new TypeResolver(this.builder.BuildServiceProvider());

    public void Register(Type service, Type implementation) =>
        this.builder.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation) => this.builder.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));

        this.builder.AddSingleton(service, (_) => factory());
    }
}
