using Application.Abstractions.Ports.Contracts;
using Domain.Abstractions.Events;
using Infrastructure.Kafka.Config;
using Microsoft.Extensions.Options;


namespace Infrastructure.Kafka;
public class KafkaEventObserver : IAdapterEventObserver
{
    private readonly IEventProducer _eventProducer;
    private readonly string _topic;

    public KafkaEventObserver(IEventProducer eventProducer, IOptions<KafkaConfig> config)
    {
        _eventProducer = eventProducer;
        _topic = config.Value.Topic;
    }

    public async Task OnEventStored(BaseEvent eventStored)
    {
        await _eventProducer.ProduceAsync(_topic, eventStored);
        return;
    }
}
