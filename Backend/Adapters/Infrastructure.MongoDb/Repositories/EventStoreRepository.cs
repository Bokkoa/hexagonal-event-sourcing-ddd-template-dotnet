using MongoDB.Driver;
using Application.Abstractions.Ports.Repositories;
using Infrastructure.MongoDb.Events;
using Domain.Events;
using Infrastructure.MongoDb.Mappers;



namespace Infrastructure.MongoDb.Repositories;
public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<MongoDbEventModel> _eventStoreCollection;
    public EventStoreRepository(IMongoCollection<MongoDbEventModel> eventStoreCollection)
    {
        _eventStoreCollection = eventStoreCollection;
    }

    public async Task<List<EventModel>> FindAllAsync()
    {
        var events = await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        return events.Select(EventModelMapper.ToEventModel).ToList();

    }

    public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        Console.WriteLine("@REPOOO");
        Console.WriteLine("@@@@@@@@@@@");
        var events = await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
        Console.WriteLine("@@@@@@@@@@@");

        return events.Select(EventModelMapper.ToEventModel).ToList();
    }

    public async Task SaveAsync(EventModel @event)
    {
        var mongoDbEvent = EventModelMapper.ToMongoDbEventModel(@event);
        await _eventStoreCollection.InsertOneAsync(mongoDbEvent).ConfigureAwait(false);
    }
}
