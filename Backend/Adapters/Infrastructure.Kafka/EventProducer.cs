
using Application.Abstractions;
using Confluent.Kafka;

namespace Infrastructure.Kafka;
public class EventProducer : IEventProducer
{
    private readonly ProducerConfig _config;
    public Task ProduceAsync<T>(string topic, T @event) where T : global::Domain.Abstractions.Events.BaseEvent
    {
        throw new NotImplementedException();
    }
}
