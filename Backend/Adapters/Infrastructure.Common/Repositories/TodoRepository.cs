using Application.Abstractions.Ports.Repositories;
using Domain.Models;
using Infrastructure.Common.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Repositories;
public class TodoRepository : ITodoRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public TodoRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(TodoModel todo)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();

        context.Todos.Add(todo);

        await context.SaveChangesAsync();
    }

    public async Task<List<TodoModel>> ListWithFoosAsync()
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Todos
                        .AsNoTracking() // faster because we dont need to track changes
                        .Include(p => p.Foos).AsNoTracking()
                        .Where(x => x.Foos != null && x.Foos.Any())
                        .ToListAsync();
    }
}
