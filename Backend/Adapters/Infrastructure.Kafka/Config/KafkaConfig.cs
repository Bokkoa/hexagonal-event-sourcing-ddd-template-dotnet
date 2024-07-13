namespace Infrastructure.Kafka.Config;
public class KafkaConfig
{
    public string Topic { get; set; }
    public ConsumerConfig ConsumerConfig { get; set; }
    public ProducerConfig ProducerConfig { get; set; }
}

public class ConsumerConfig
{
    public string GroupId { get; set; }
    public string BootstrapServers { get; set; }
    public bool? EnableAutoCommit { get; set; }
    public bool AllowAutoCreateTopics { get; set; }
}

public class ProducerConfig
{
    public string BootstrapServers { get; set; }
}