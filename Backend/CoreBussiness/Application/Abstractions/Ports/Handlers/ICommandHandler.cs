using Application.Commands;

namespace Application.Abstractions.Ports.Handlers;
public interface ICommandHandler
{
    Task HandleAsync(NewTodoCommand command);
}
