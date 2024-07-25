using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RubeesEat.Model;

namespace RubeesEat.Controllers;

[ApiController]
[Route("/api/todos")]
public class TodoController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;

    public TodoController(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [Route("")]
    [HttpGet]
    public IEnumerable<TodoItem> GetAll()
    {
        return _todoRepository.GetAll();
    }

    [Route("")]
    [HttpPost]
    public Results<BadRequest<string>, RedirectHttpResult> Create([FromForm] string name)
    {
        if (string.IsNullOrEmpty(name))
            return TypedResults.BadRequest("TODO name is invalid.");

        var todoItem = TodoItem.Create(name);
        _todoRepository.Add(todoItem);

        return TypedResults.Redirect("/Todo");
    }
}
