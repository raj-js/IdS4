using MediatR;
using System.Collections.Generic;

namespace IdS4.Abstraction
{
    public interface IHasEvents
    {
        IReadOnlyCollection<INotification> Events { get; }

        void AddEvent(INotification @event);

        void RemoveEvent(INotification @event);

        void ClearEvent();
    }
}
