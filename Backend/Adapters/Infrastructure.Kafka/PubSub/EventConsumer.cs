using Application.Abstractions.Ports.Contracts;
using Application.Abstractions.Ports.Handlers;
using Application.Converters;
using Confluent.Kafka;
using Domain.Abstractions.Events;
using Infrastructure.Kafka.Config;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Kafka.PubSub;
public class EventConsumer : IEventConsumer
{
    private readonly KafkaConfig _config;
    private readonly IEventHandler _eventHandler;

    public EventConsumer(IOptions<KafkaConfig> config, IEventHandler eventHandler)
    {
        _config = config.Value;
        _eventHandler = eventHandler;
    }

    public void Consume(string topic)
    {

        var consumerConfig = new Confluent.Kafka.ConsumerConfig
        {
            GroupId = _config.ConsumerConfig.GroupId,
            BootstrapServers = _config.ConsumerConfig.BootstrapServers,
            EnableAutoCommit = _config.ConsumerConfig.EnableAutoCommit,
            AllowAutoCreateTopics = _config.ConsumerConfig.AllowAutoCreateTopics
        };

        using var consumer = new ConsumerBuilder<string, string>(consumerConfig)
                                  .SetKeyDeserializer(Deserializers.Utf8)
                                  .SetValueDeserializer(Deserializers.Utf8)
                                  .Build();

        consumer.Subscribe(topic);

        while (true)
        {
            var consumerResult = consumer.Consume();

            if (consumerResult?.Message == null) continue;

            var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };

            var @event = JsonSerializer.Deserialize<BaseEvent>(consumerResult.Message.Value, options);
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });

            if (handlerMethod == null)
            {
                throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");
            }

            handlerMethod.Invoke(_eventHandler, new object[] { @event });
            consumer.Commit(consumerResult);
        }
    }
}
