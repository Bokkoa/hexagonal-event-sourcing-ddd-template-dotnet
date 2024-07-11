﻿namespace Domain.Abstractions.Events;
public abstract class BaseEvent : Message
{
    protected BaseEvent(string type)
    {
        Type = type;
    }
    public int Version { get; set; }
    public string Type { get; set; }
}
