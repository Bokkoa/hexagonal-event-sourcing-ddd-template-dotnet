using Application.Abstractions;

namespace Application.Commands;
public class NewTodoCommand : BaseCommand
{
    public string Foo { get; set; }
    public string Bar { get; set; }
}
