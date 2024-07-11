using Application.Abstractions;
using Domain.Abstractions.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;
public class EventSourcingHandler<T> : IEventSourcingHandler<T> where T: class
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
        var aggregate = AggregateFactory.CreateAggregate<T>();
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
        await _eventStore.SaveEventAsync<AggregateRoot>(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }
}
