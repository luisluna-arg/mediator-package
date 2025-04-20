namespace MediatorPackage.Exceptions;

/// <summary>
/// Exception thrown when a command executor interface for a given command type is not found.
/// </summary>
public class CommandExecutorInterfaceNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutorInterfaceNotFoundException"/> class.
    /// </summary>
    public CommandExecutorInterfaceNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutorInterfaceNotFoundException"/> class
    /// with the specified command type.
    /// </summary>
    /// <param name="commandType">The command type for which the executor interface was not found.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="commandType"/> is <c>null</c>.</exception>
    public CommandExecutorInterfaceNotFoundException(Type commandType)
        : base($"Command executor interface for command \"{commandType?.FullName}\" not found")
    {
        if (commandType is null)
        {
            throw new ArgumentNullException(nameof(commandType));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutorInterfaceNotFoundException"/> class
    /// with a custom error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public CommandExecutorInterfaceNotFoundException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutorInterfaceNotFoundException"/> class
    /// with a custom error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public CommandExecutorInterfaceNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
