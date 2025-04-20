namespace MediatorPackage.Utils;

/// <summary>
/// Provides utility methods for working with command-related types.
/// </summary>
public static class TypeUtils
{
    /// <summary>
    /// Gets the <see cref="ICommandExecutor{TCommand}"/> interface implemented by the specified type, if any.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <returns>
    /// The generic <see cref="ICommandExecutor{TCommand}"/> interface implemented by the type, or <c>null</c> if not found.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <c>null</c>.</exception>
    public static Type? GetCommandInterface(Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var openGeneric = typeof(ICommandExecutor<>);
        foreach (var typeInterface in type.GetInterfaces())
        {
            if (typeInterface.IsGenericType && typeInterface.GetGenericTypeDefinition() == openGeneric)
            {
                return typeInterface;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the base name (without generic suffix) of the generic type definition.
    /// </summary>
    /// <param name="type">The generic type to inspect.</param>
    /// <returns>The base name of the generic type, excluding any backtick and arity suffix.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is <c>null</c>.</exception>
    public static string GetTypeBaseName(Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return type.GetGenericTypeDefinition().Name.Split('`')[0];
    }
}
