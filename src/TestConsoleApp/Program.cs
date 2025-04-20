using MediatorPackage;
using MediatorPackage.Utils;
using Microsoft.Extensions.DependencyInjection;
using TestConsoleApp.Commands;

var services = new ServiceCollection();

services.AddMediator([typeof(Program).Assembly]);

var serviceProvider = services.BuildServiceProvider();
var mediator = serviceProvider.GetService<Mediator>()!;
var command = new HelloCommand("Luis");
await mediator.Execute(command);
