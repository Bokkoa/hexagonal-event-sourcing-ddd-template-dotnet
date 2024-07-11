using Domain.Models;

namespace Application.Abstractions.Ports.Repositories;

public interface IFooRepository
{
    Task CreateAsync(FooModel foo);
    Task UpdateAsync(FooModel foo);
    Task<FooModel> GetByIdAsync(Guid fooId);


}
