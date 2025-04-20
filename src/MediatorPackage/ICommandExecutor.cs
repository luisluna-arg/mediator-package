namespace MediatorPackage;

/// <summary>
/// Defines a command executor for commands that do not return a result.
/// </summary>
/// <typeparam name="TCommand">The type of the command to execute.</typeparam>
public interface ICommandExecutor<TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Executes the specified command asynchronously.
    /// </summary>
    /// <param name="request">The command to execute.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Execute(TCommand request);
}

/// <summary>
/// Defines a command executor for commands that return a result.
/// </summary>
/// <typeparam name="TCommand">The type of the command to execute.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
public interface ICommandExecutor<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Executes the specified command asynchronously and returns a result.
    /// </summary>
    /// <param name="request">The command to execute.</param>
    /// <returns>A task that represents the asynchronous operation and contains the result of the command.</returns>
    Task<TResult> Execute(ICommand<TResult> request);
}
