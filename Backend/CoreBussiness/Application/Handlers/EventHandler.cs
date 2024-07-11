using Application.Abstractions.Ports.Handlers;
using Application.Abstractions.Ports.Repositories;
using Domain.Models;
using Domain.Modules.Todos.Events;

namespace Application.Handlers;
public class EventHandler : IEventHandler
{
    private readonly ITodoRepository _todoRepository;
    private readonly IFooRepository _fooRepository;

    public EventHandler(IFooRepository fooRepository, ITodoRepository todoRepository)
    {
        _fooRepository = fooRepository;
        _todoRepository = todoRepository;
    }

    public async Task On(TodoCreatedEvent @event)
    {
        var todo = new TodoModel
        {
            Id = @event.Id,
            Author = @event.Author,
            DateEmmited = @event.DateEmmited,
        };

        await _todoRepository.CreateAsync(todo);
    }

    public async Task On(FooUpdatedEvent @event)
    {
        var foo = await _fooRepository.GetByIdAsync(@event.Id);
        if (foo == null) return;

        foo.Author = @event.Author;
        await _fooRepository.UpdateAsync(foo);
    }
}
