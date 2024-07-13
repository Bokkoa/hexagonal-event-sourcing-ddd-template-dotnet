using Domain.Abstractions.Events;

namespace Application.Abstractions.Ports.Contracts;
public interface IAdapterEventObserver
{
    Task OnEventStored(BaseEvent eventStored);
}
