using Domain.Events;
using Infrastructure.MongoDb.Events;

namespace Infrastructure.MongoDb.Mappers;

public static class EventModelMapper
{
    public static MongoDbEventModel ToMongoDbEventModel(EventModel eventModel)
    {
        return new MongoDbEventModel
        {
            Id = eventModel.Id,
            TimeStamp = eventModel.TimeStamp,
            AggregateIdentifier = eventModel.AggregateIdentifier,
            AggregateType = eventModel.AggregateType,
            Version = eventModel.Version,
            EventType = eventModel.EventType,
            EventData = eventModel.EventData
        };
    }

    public static EventModel ToEventModel(MongoDbEventModel mongoDbEventModel)
    {
        return new EventModel
        {
            Id = mongoDbEventModel.Id,
            TimeStamp = mongoDbEventModel.TimeStamp,
            AggregateIdentifier = mongoDbEventModel.AggregateIdentifier,
            AggregateType = mongoDbEventModel.AggregateType,
            Version = mongoDbEventModel.Version,
            EventType = mongoDbEventModel.EventType,
            EventData = mongoDbEventModel.EventData
        };
    }
}