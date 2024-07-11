using Domain.Modules.Todos.Events;


namespace Application.Abstractions.Ports.Handlers;
public interface IEventHandler
{
    Task On(TodoCreatedEvent @event);
    Task On(FooUpdatedEvent @event);
}
