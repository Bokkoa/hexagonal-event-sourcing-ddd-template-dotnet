using Domain.Modules.Todos.Events;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Abstractions.Events;

[BsonKnownTypes(typeof(TodoCreatedEvent))]
public abstract class BaseEvent : Message
{
    protected BaseEvent(string type)
    {
        Type = type;
    }
    public int Version { get; set; }
    public string Type { get; set; }
}
