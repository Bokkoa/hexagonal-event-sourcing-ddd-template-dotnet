using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.DataAccess;
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<TodoModel> Todos { get; set; }
    public DbSet<FooModel> Foos { get; set; }

}
