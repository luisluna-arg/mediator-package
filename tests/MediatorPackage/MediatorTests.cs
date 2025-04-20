using MediatorPackage.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorPackage.Tests;

public class MediatorTests
{
    [Fact]
    public async Task Execute_CommandWithoutResult_ResolvesAndExecutes()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<ICommandExecutor<TestCommand>, TestCommandExecutor>();
        var provider = services.BuildServiceProvider();
        var mediator = new Mediator(provider);

        var command = new TestCommand();

        // Act & Assert (no exception)
        await mediator.Execute(command);
        Assert.True(command.Executed);
    }

    [Fact]
    public async Task Execute_CommandWithResult_ResolvesAndReturns()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<ICommandExecutor<TestQuery, int>, TestQueryExecutor>();
        var provider = services.BuildServiceProvider();
        var mediator = new Mediator(provider);

        var command = new TestQuery();

        // Act
        var result = await mediator.Execute<TestQuery, int>(command);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task Execute_CommandWithoutExecutor_ThrowsException()
    {
        // Arrange
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var mediator = new Mediator(provider);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<CommandExecutorInterfaceNotFoundException>(
            () => mediator.Execute(new UnregisteredCommand()));
        Assert.Equal($"Command executor interface for command \"{typeof(UnregisteredCommand)?.FullName}\" not found", ex.Message);
    }

    [Fact]
    public async Task Execute_CommandWithResult_WithoutExecutor_ThrowsException()
    {
        // Arrange
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var mediator = new Mediator(provider);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<CommandExecutorInterfaceNotFoundException>(
            () => mediator.Execute<UnregisteredQuery, string>(new UnregisteredQuery()));
        Assert.Equal($"Command executor interface for command \"{typeof(UnregisteredQuery)?.FullName}\" not found", ex.Message);
    }

    // --- Test doubles ---

    private class TestCommand : ICommand
    {
        public bool Executed { get; set; }
    }

    private class TestCommandExecutor : ICommandExecutor<TestCommand>
    {
        public Task Execute(TestCommand request)
        {
            request.Executed = true;
            return Task.CompletedTask;
        }
    }

    private class TestQuery : ICommand<int> { }

    private class TestQueryExecutor : ICommandExecutor<TestQuery, int>
    {
        public Task<int> Execute(ICommand<int> request)
        {
            return Task.FromResult(42);
        }
    }

    private class UnregisteredCommand : ICommand { }

    private class UnregisteredQuery : ICommand<string> { }
}
