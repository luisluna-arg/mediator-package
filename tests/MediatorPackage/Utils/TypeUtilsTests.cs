namespace MediatorPackage.Utils.Tests;

public class TypeUtilsTests
{
    [Fact]
    public void GetCommandInterface_ReturnsInterface_WhenImplemented()
    {
        // Act
        var result = TypeUtils.GetCommandInterface(typeof(MyExecutor));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(typeof(ICommandExecutor<MyCommand>), result);
    }

    [Fact]
    public void GetCommandInterface_ReturnsNull_WhenNotImplemented()
    {
        // Act
        var result = TypeUtils.GetCommandInterface(typeof(string)); // string doesn't implement ICommandExecutor<>

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetCommandInterface_Throws_WhenTypeIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => TypeUtils.GetCommandInterface(null!));
    }

    [Fact]
    public void GetTypeBaseName_ReturnsBaseName_WithoutGenericArity()
    {
        // Arrange
        var type = typeof(GenericType<string, int>);

        // Act
        var result = TypeUtils.GetTypeBaseName(type);

        // Assert
        Assert.Equal("GenericType", result);
    }

    [Fact]
    public void GetTypeBaseName_Throws_WhenTypeIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => TypeUtils.GetTypeBaseName(null!));
    }

    // test doubles

    private class MyCommand : ICommand { }

    private class MyExecutor : ICommandExecutor<MyCommand>
    {
        public Task Execute(MyCommand request) => Task.CompletedTask;
    }

    private class GenericType<T1, T2> { }
}
