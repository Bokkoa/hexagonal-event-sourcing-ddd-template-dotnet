using Domain.Models;

namespace Application.Abstractions.Ports.Repositories;
public interface ITodoRepository
{
    Task CreateAsync(TodoModel model);
    Task<List<TodoModel>> ListWithFoosAsync();

}
