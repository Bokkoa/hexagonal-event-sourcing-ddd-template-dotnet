using Api.Dtos;
using Application.Abstractions;
using Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewTodoController : ControllerBase
{
    private readonly ILogger<NewTodoController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public NewTodoController(ICommandDispatcher commandDispatcher, ILogger<NewTodoController> logger)
    {
        _commandDispatcher = commandDispatcher;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> NewTodoAsync(NewTodoCommand command)
    {
        var id = Guid.NewGuid();
        try
        {

            await _commandDispatcher.SendAsync(command);

            return StatusCode(StatusCodes.Status201Created, new NewTodoResponse
            {
                Message = "New Todo created!"
            });
        } catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new todo!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);
            return StatusCode(StatusCodes.Status500InternalServerError, new NewTodoResponse
            {
                Id = id,
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }
}
