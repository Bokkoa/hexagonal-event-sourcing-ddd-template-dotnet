using Domain.Abstractions.Aggregates;

namespace Application.Abstractions;
public interface IEventSourcingHandler<T>
{
    Task SaveAsync(AggregateRoot aggregate);

    Task<T> GetByIdAsync(Guid id);
    Task RepublishEventsAsync();
}
