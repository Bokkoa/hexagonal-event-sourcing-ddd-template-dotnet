using Api.Dtos;
using Application.Abstractions.Ports.Contracts;
using Application.Queries;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TodosLookupController : ControllerBase
{
    private readonly ILogger<TodosLookupController> _logger;
    private readonly IQueryDispatcher<TodoModel> _queryDispatcher;

    public TodosLookupController(IQueryDispatcher<TodoModel> queryDispatcher, ILogger<TodosLookupController> logger)
    {
        _queryDispatcher = queryDispatcher;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodosAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindAllTodosQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all posts!";
            return ErrorMethod(ex, SAFE_ERROR_MESSAGE);
        }
    }


    private ActionResult NormalResponse(List<TodoModel> todos)
    {
        if (todos == null || !todos.Any())
        {
            return NoContent();
        }

        var count = todos.Count;

        return Ok(new TodosLookupResponse
        {
            Todos = todos,
            Message = $"Successfully returned {count} todos{(count > 1 ? "s" : string.Empty)}"
        });
    }

    private ActionResult ErrorMethod(Exception ex, string safeErrorMessage)
    {
        _logger.LogError(ex, safeErrorMessage);

        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        {
            Message = safeErrorMessage,
        });
    }

}
