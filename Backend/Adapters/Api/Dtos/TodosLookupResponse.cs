using Domain.Models;

namespace Api.Dtos;

public class TodosLookupResponse : BaseResponse
{
    public List<TodoModel> Todos { get; set; }
}
