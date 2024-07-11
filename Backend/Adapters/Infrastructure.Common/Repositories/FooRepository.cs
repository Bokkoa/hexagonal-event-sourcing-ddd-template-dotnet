using Application.Abstractions.Ports.Repositories;
using Domain.Models;
using Infrastructure.Common.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;


namespace Infrastructure.Common.Repositories;
public class FooRepository : IFooRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public FooRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(FooModel foo)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        context.Foos.Add(foo);
        _ = await context.SaveChangesAsync();
    }

    public async Task<FooModel> GetByIdAsync(Guid fooId)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Foos.FirstOrDefaultAsync(x => x.Id == fooId);
    }

    public async Task UpdateAsync(FooModel foo)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        context.Foos.Update(foo);

        _ = await context.SaveChangesAsync();
    }
}
