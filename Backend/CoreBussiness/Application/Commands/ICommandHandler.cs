namespace Application.Commands;
public interface ICommandHandler
{
    Task HandleAsync(NewTodoCommand command);
}
