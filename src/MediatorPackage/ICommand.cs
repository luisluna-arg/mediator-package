namespace MediatorPackage;

/// <summary>
/// Represents a command with no return value.
/// </summary>
public interface ICommand
{
}

/// <summary>
/// Represents a command that returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
public interface ICommand<TResult>
{
}
