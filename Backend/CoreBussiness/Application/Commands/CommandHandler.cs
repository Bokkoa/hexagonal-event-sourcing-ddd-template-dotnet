using Application.Abstractions.Ports.Handlers;
using Domain.Modules.Todos.Aggregates;

namespace Application.Commands;
public class CommandHandler : ICommandHandler
{
    private readonly IEventSourcingHandler<TodoAggregate> _eventSourcingHandler;

    public CommandHandler(IEventSourcingHandler<TodoAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task HandleAsync(NewTodoCommand command)
    {
        var aggregate = new TodoAggregate(command.Id, command.Bar, command.Foo);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }
}
