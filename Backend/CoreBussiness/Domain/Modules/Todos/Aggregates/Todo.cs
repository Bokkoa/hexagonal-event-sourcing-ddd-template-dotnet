using Domain.Abstractions.Aggregates;
using Domain.Modules.Todos.Entities;
using Domain.Modules.Todos.Events;
using Domain.Modules.Todos.ValueObjects;

namespace Domain.Modules.Todos.Aggregates;
public class Todo: AggregateRoot
{
    private bool _active;
    private string _author;
    private readonly Dictionary<Guid, Tuple<string, string>> _foos = new();
    public Foo Foo { get; private set; } // entity
    public Bar Bar { get; private set; } // record value object


    public bool Active
    {
        get => _active; set => _active = value;
    }

    public Todo()
    {

    }

    public Todo(Guid id, string author, string foo)
    {
        RaiseEvent(new TodoCreatedEvent
        {
            Id = id,
            Author = author,
            Foo = new Foo (foo, "a@example.com"),
            DateEmmited = DateTime.Now,
        });
    }

    public void Apply(TodoCreatedEvent @event)
    {
        _id = @event.Id;
        _active = true;
        _author = @event.Author;
    }

    public void EditFoo(string foo)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit the foo of an inactive post!");
        }

        if (string.IsNullOrEmpty(foo))
        {
            throw new InvalidOperationException($"The value of {nameof(foo)} cannot be null or empty.");
        }

        RaiseEvent(new FooUpdatedEvent
        {
            Id = _id,
            Email = foo
        });
    }

    public void Apply(FooUpdatedEvent @event)
    {
        _id = @event.Id;
    }
}
