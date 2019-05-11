using RajsLibs.Key;
using System;

namespace RajsLibs.EventBus.Event
{
    public interface IEvent : IKey<string>
    {
        EventState State { get; }
        DateTimeOffset CreateTimeStamp { get; }
    }
}
