using MediatorPackage.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorPackage;

/// <summary>
/// Represents the mediator responsible for executing commands using registered command executors.
/// </summary>
public class Mediator
{
    private readonly IServiceScope scope;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mediator"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve command executors.</param>
    public Mediator(IServiceProvider serviceProvider)
    {
        scope = serviceProvider.CreateScope();
    }

    /// <summary>
    /// Executes a command that does not return a result.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command to execute.</typeparam>
    /// <param name="command">The command instance to execute.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="CommandExecutorInterfaceNotFoundException">
    /// Thrown when no suitable executor is found for the specified command type.
    /// </exception>
    public async Task Execute<TCommand>(TCommand command)
       where TCommand : ICommand
    {
        var type = GetExecutorInterface(typeof(TCommand));
        if (type == null)
        {
            throw new CommandExecutorInterfaceNotFoundException(typeof(TCommand));
        }

        var service = scope.ServiceProvider.GetService(type);
        if (service is ICommandExecutor<TCommand> executor)
        {
            await executor.Execute(command).ConfigureAwait(false);
            return;
        }

        throw new CommandExecutorInterfaceNotFoundException(typeof(TCommand));
    }

    /// <summary>
    /// Executes a command that returns a result.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command to execute.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
    /// <param name="command">The command instance to execute.</param>
    /// <returns>A task that represents the asynchronous operation and contains the result of the command.</returns>
    /// <exception cref="CommandExecutorInterfaceNotFoundException">
    /// Thrown when no suitable executor is found for the specified command type.
    /// </exception>
    public async Task<TResult> Execute<TCommand, TResult>(TCommand command)
        where TCommand : ICommand<TResult>
    {
        var type = GetExecutorInterface(typeof(TCommand));
        if (type == null)
        {
            throw new CommandExecutorInterfaceNotFoundException(typeof(TCommand));
        }

        var service = scope.ServiceProvider.GetService(type);
        if (service is ICommandExecutor<TCommand, TResult> executor)
        {
            return await executor.Execute(command).ConfigureAwait(false);
        }

        throw new CommandExecutorInterfaceNotFoundException(typeof(TCommand));
    }

    /// <summary>
    /// Gets the executor interface type for the given command type.
    /// </summary>
    /// <param name="commandType">The type of the command.</param>
    /// <returns>
    /// The corresponding executor interface type if found; otherwise, <c>null</c>.
    /// </returns>
    private static Type? GetExecutorInterface(Type commandType)
    {
        foreach (var loopIface in commandType.GetInterfaces())
        {
            if (loopIface.IsGenericType && loopIface.GetGenericTypeDefinition() == typeof(ICommand<>))
            {
                return typeof(ICommandExecutor<,>).MakeGenericType(commandType, loopIface.GetGenericArguments()[0]);
            }
        }

        if (typeof(ICommand).IsAssignableFrom(commandType))
        {
            return typeof(ICommandExecutor<>).MakeGenericType(commandType);
        }

        return null;
    }
}
