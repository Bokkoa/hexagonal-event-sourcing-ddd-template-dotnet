using Application.Abstractions.Ports.Handlers;
using Domain.Modules.Todos.Aggregates;

namespace Application.Commands;
public class CommandHandler : ICommandHandler
{
    private readonly IEventSourcingHandler<Todo> _eventSourcingHandler;

    public CommandHandler(IEventSourcingHandler<Todo> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(NewTodoCommand command)
    {
        var aggregate = new Todo(command.Id, command.Bar, command.Foo);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}
