using Application.Abstractions.Ports.Handlers;
using Application.Abstractions.Ports.Repositories;
using Domain.Models;

namespace Application.Queries;
public class QueryHandler : IQueryHandler
{
    private readonly ITodoRepository _todoRepository;

    public QueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    public async Task<List<TodoModel>> HandleAsync(FindAllTodosQuery query)
    {
        return await _todoRepository.ListWithFoosAsync();

    }

    public async Task<List<TodoModel>> HandleAsync(ListWithFoosQuery query)
    {
        return await _todoRepository.ListWithFoosAsync();
    }
}
