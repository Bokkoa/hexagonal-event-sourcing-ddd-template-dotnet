﻿using Domain.Abstractions.Events;

namespace Application.Abstractions;
public interface IEventStore
{
    Task SaveEventAsync<TAggregate>(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
    Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
    Task<List<Guid>> GetAggregateIdAsync();
}
