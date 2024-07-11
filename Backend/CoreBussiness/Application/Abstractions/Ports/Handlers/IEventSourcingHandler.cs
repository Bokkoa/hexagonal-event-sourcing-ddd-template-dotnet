using Domain.Abstractions.Aggregates;

namespace Application.Abstractions.Ports.Handlers;
public interface IEventSourcingHandler<T>
{
    Task SaveAsync(AggregateRoot aggregate);

    Task<T> GetByIdAsync(Guid id);
    Task RepublishEventsAsync();
}
