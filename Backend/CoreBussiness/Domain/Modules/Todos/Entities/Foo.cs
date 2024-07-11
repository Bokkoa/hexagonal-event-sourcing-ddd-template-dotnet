using Domain.Abstractions.Entities;

namespace Domain.Modules.Todos.Entities;
public class Foo : Entity
{
    public Foo(string firstName, string email)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        Email = email;
    }
    public string FirstName { get; set; }
    public string Email { get; set; }
}
