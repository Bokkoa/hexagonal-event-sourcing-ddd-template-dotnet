using Domain.Abstractions.Events;

namespace Application.Abstractions;
public interface IEventProducer
{
    Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;

}
