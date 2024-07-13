using Application.Queries;
using Domain.Models;

namespace Application.Abstractions.Ports.Handlers;
public interface IQueryHandler
{
    Task<List<TodoModel>> HandleAsync(FindAllTodosQuery query);
    Task<List<TodoModel>> HandleAsync(ListWithFoosQuery query);
}
