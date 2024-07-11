using Domain.Abstractions.Events;

namespace Domain.Modules.Todos.Events;
public class FooUpdatedEvent : BaseEvent
{
    public FooUpdatedEvent() : base(nameof(FooUpdatedEvent)) { }

    public string Foo { get; set; }
    public string Author { get; set; }
}
