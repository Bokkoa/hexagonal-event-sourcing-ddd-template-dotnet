namespace Application.Abstractions.Ports.Contracts;
public interface IEventConsumer
{
    void Consume(string topic);
}
