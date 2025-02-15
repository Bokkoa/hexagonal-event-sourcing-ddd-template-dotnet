﻿using Application.Abstractions.Ports.Contracts;
using Application.Abstractions.Ports.Repositories;
using Application.Exceptions;
using Domain.Abstractions.Events;
using Domain.Events;


namespace Infrastructure.MongoDb.Stores;
public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly IEventProducer _eventProducer;
    private readonly IEnumerable<IAdapterEventObserver> _adaptersObservers;

    public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer, IEnumerable<IAdapterEventObserver> adaptersObservers)
    {
        _eventStoreRepository = eventStoreRepository;
        _eventProducer = eventProducer;
        _adaptersObservers = adaptersObservers;
    }

    public async Task<List<Guid>> GetAggregateIdAsync()
    {
        var eventStream = await _eventStoreRepository.FindAllAsync();

        if (eventStream == null || !eventStream.Any())
        {
            throw new ArgumentNullException(nameof(eventStream), "Could not retrieve event stream from the event store!");
        }

        return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (eventStream == null || !eventStream.Any())
        {
            throw new AggregateNotFoundException("Incorrect post ID provided");
        }

        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
    }

    public async Task SaveEventAsync<TAggregate>(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
        {
            throw new ConcurrencyException();
        }

        var version = expectedVersion;
        var aggregateType = typeof(TAggregate).Name;
        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = aggregateType,
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            await _eventStoreRepository.SaveAsync(eventModel);

            foreach(var adapter in _adaptersObservers)
            {
                // emiting message of store event between adapter components
                await adapter.OnEventStored(@event);
            }
        }
    }
}
