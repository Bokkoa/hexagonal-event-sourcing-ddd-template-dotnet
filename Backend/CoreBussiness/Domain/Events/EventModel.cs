﻿
using Domain.Abstractions.Events;

namespace Domain.Events;
public class EventModel
{
    public string Id { get; set; }

    public DateTime TimeStamp { get; set; }
    public Guid AggregateIdentifier { get; set; }

    public string AggregateType { get; set; }

    public int Version { get; set; }
    public string EventType { get; set; }
    public BaseEvent EventData { get; set; }
}
