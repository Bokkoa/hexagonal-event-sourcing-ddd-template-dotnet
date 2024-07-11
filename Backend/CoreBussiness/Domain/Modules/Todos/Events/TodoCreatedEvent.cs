using Domain.Abstractions.Events;
using Domain.Modules.Todos.Entities;


namespace Domain.Modules.Todos.Events;
public class TodoCreatedEvent : BaseEvent
{
    public TodoCreatedEvent() : base(nameof(TodoCreatedEvent)) { }

    public string Author { get; set; }
    public Foo Foo { get; set; }
    public DateTime DateEmmited { get; set; }

}
