using Application.Abstractions.Ports.Contracts;
using Confluent.Kafka;
using Infrastructure.Kafka.Config;
using Infrastructure.Kafka.PubSub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;
public static class InfrastructureKafkaAdapterDependency
{
    public static IServiceCollection InjectKafkaDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaConfig>(configuration.GetSection("KafkaConfig"));

        // PRODUCER
        services.AddScoped<IEventProducer, EventProducer>();

        //// CONSUMER
        services.AddScoped<IEventConsumer, EventConsumer>();

        //// HOSTED SERVICE
        services.AddHostedService<ConsumerHostedService>();

        // OBSERVER BETWEEN ADAPTERS
        services.AddScoped<IAdapterEventObserver, KafkaEventObserver>();

        return services;
    }

}
