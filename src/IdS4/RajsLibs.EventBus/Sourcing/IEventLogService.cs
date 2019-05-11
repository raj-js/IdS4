using RajsLibs.EventBus.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RajsLibs.EventBus.Sourcing.Services
{
    public interface IEventLogService
    {
        Task<IEnumerable<EventLog>> RetrievePendingEventAsync();

        Task SaveAsync(IEvent @event);

        Task MarkAsInProgressAsync(string id);

        Task MarkAsPublishedAsync(string id);

        Task MarkAsFailedAsync(string id);
    }
}
