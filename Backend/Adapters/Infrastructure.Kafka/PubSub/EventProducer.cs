using Application.Abstractions.Ports.Contracts;
using Confluent.Kafka;
using Domain.Abstractions.Events;
using Infrastructure.Kafka.Config;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Kafka.PubSub;
public class EventProducer : IEventProducer
{
    private readonly KafkaConfig _config;

    public EventProducer(IOptions<KafkaConfig> cfg)
    {
        _config = cfg.Value;
    }
    public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
    {
        var producerConfig = new Confluent.Kafka.ProducerConfig
        {
            BootstrapServers = _config.ProducerConfig.BootstrapServers
        };
        using var producer = new ProducerBuilder<string, string>(producerConfig)
               .SetKeySerializer(Serializers.Utf8)
               .SetValueSerializer(Serializers.Utf8)
               .Build();

        var eventMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event, @event.GetType())
        };

        var deliveryResult = await producer.ProduceAsync(topic, @eventMessage);

        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
        {
            throw new Exception($"Could not produce {@event.GetType().Name} message to topic - {topic} due to the following reason: {deliveryResult.Message}");
        }
    }
}
