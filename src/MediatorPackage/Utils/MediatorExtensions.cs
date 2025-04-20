using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorPackage.Utils;

/// <summary>
/// Provides extension methods for registering the <see cref="Mediator"/> and its executors in a service collection.
/// </summary>
public static class MediatorExtensions
{
    /// <summary>
    /// Registers the <see cref="Mediator"/> and all implementations of <see cref="ICommandExecutor{TCommand}"/> and
    /// <see cref="ICommandExecutor{TCommand, TResult}"/> found in the provided assemblies.
    /// </summary>
    /// <param name="serviceCollection">The service collection to register services with.</param>
    /// <param name="assemblies">Assemblies to scan for command executors.</param>
    public static void AddMediator(this IServiceCollection serviceCollection, Assembly[] assemblies)
    {
        RegisterExecutors(serviceCollection, assemblies ?? []);
        serviceCollection.AddScoped(provider => new Mediator(provider));
    }

    /// <summary>
    /// Registers all implementations of <see cref="ICommandExecutor{TCommand}"/> and
    /// <see cref="ICommandExecutor{TCommand, TResult}"/> found in the given assemblies.
    /// </summary>
    /// <param name="serviceCollection">The service collection to register services with.</param>
    /// <param name="assemblies">Assemblies to scan for executor implementations.</param>
    private static void RegisterExecutors(IServiceCollection serviceCollection, Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                foreach (var iface in type.GetInterfaces())
                {
                    if (!iface.IsGenericType)
                    {
                        continue;
                    }

                    var def = iface.GetGenericTypeDefinition();
                    if (def == typeof(ICommandExecutor<>) || def == typeof(ICommandExecutor<,>))
                    {
                        serviceCollection.AddScoped(iface, type);
                        break;
                    }
                }
            }
        }
    }
}
