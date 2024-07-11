namespace Application.Abstractions;
internal interface IEventConsumer
{
    void Consume(string topic);
}
