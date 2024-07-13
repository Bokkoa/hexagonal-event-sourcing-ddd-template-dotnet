using Application.Abstractions.Ports.Contracts;
using Infrastructure.Kafka.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Infrastructure.Kafka.PubSub;
public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly KafkaConfig _config;

    public ConsumerHostedService(IServiceProvider serviceProvider, ILogger<ConsumerHostedService> logger, IOptions<KafkaConfig> config)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = config.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event consumer service running"); ;

        using (IServiceScope scope = _serviceProvider.CreateScope())
        {

            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            var topic = _config.Topic;
            Task.Run(() => eventConsumer.Consume(topic), cancellationToken);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Consumer service stopped."); ;

        return Task.CompletedTask;
    }
}
