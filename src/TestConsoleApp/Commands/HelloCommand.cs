using MediatorPackage;

namespace TestConsoleApp.Commands;

public class HelloCommand(string name) : ICommand
{
    public string Name { get; } = name;
}

public class HelloCommandExecutor() : ICommandExecutor<HelloCommand>
{
    public Task Execute(HelloCommand request)
    {
        Console.WriteLine($"Hello {request.Name}, welcome to the world!");
        return Task.CompletedTask;
    }
}