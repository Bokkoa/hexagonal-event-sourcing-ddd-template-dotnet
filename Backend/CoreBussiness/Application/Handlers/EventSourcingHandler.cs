using Application.Abstractions.Ports.Contracts;
using Application.Abstractions.Ports.Handlers;
using Domain.Abstractions.Aggregates;


namespace Application.Handlers;
public class EventSourcingHandler<T> : IEventSourcingHandler<T> where T: AggregateRoot
{
    private readonly IEventStore _eventStore;
    private readonly IEventProducer _eventProducer;

    public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
    {
        _eventStore = eventStore;
        _eventProducer = eventProducer;
    }
    public async Task<T> GetByIdAsync(Guid aggregateId)
    {
        Console.WriteLine("GET BY ID");
        var aggregate = AggregateFactory.CreateAggregate<T>();
        Console.WriteLine("@@@@@@@@@@");
        Console.WriteLine(aggregate.GetType());
        Console.WriteLine("@@@@@@@@@@");
        var events = await _eventStore.GetEventsAsync(aggregateId);

        if (events == null || !events.Any())
        {
            return aggregate;
        }

        (aggregate as dynamic).ReplayEvents(events);
        var latestVersion = events.Select(x => x.Version).Max();

        return aggregate;
    }

    public async Task RepublishEventsAsync()
    {
        Console.WriteLine("REPUBLISH");

        var aggregateIds = await _eventStore.GetAggregateIdAsync();

        if (aggregateIds == null || !aggregateIds.Any()) return;


        foreach (var aggregateId in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aggregateId);

            if (aggregate == null || !(aggregate as dynamic).Active) continue;

            var events = await _eventStore.GetEventsAsync(aggregateId);

            foreach (var @event in events)
            {
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        Console.WriteLine("saving");
         Console.WriteLine("@@@@@@@@@@");
        Console.WriteLine(aggregate.GetType());
        Console.WriteLine("@@@@@@@@@@");
        await _eventStore.SaveEventAsync<T>(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}
