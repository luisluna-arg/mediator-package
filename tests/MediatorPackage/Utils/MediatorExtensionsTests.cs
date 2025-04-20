using Microsoft.Extensions.DependencyInjection;

namespace MediatorPackage.Utils.Tests;

public class MediatorExtensionsTests
{
    [Fact]
    public void AddMediator_RegistersMediatorAndExecutors()
    {
        // Arrange
        var services = new ServiceCollection();
        var assemblies = new[] { typeof(TestCommandExecutor).Assembly };

        // Act
        services.AddMediator(assemblies);
        var provider = services.BuildServiceProvider();

        // Assert: Mediator is registered
        var mediator = provider.GetRequiredService<Mediator>();
        Assert.NotNull(mediator);

        // Assert: ICommandExecutor<TestCommand> is registered
        var executor = provider.GetService<ICommandExecutor<TestCommand>>();
        Assert.NotNull(executor);
        Assert.IsType<TestCommandExecutor>(executor);
    }
}

// Test double: command
public class TestCommand : ICommand { }

// Test double: executor
public class TestCommandExecutor : ICommandExecutor<TestCommand>
{
    public Task Execute(TestCommand request)
    {
        return Task.CompletedTask;
    }
}
