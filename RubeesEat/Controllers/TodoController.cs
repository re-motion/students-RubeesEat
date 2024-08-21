﻿using Microsoft.AspNetCore.Http.HttpResults;
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


    [Route("delete")]
    [HttpPost]
    public Results<BadRequest<string>, RedirectHttpResult> Delete([FromForm] string guid)
    {
        _todoRepository.Delete(new Guid(guid));
        return TypedResults.Redirect("/Todo");
    }

    [Route("checkbox")]
    [HttpPost]
    public Results<BadRequest<string>, RedirectHttpResult> Checkbox([FromForm] Guid guid, [FromForm] bool checkbox)
    {
        var todo = _todoRepository.GetById(guid);
        if (todo == null)
            return TypedResults.BadRequest("Todo not found");

        try
        {
            _todoRepository.Update(todo.WithIsCompleted(checkbox));
        }
        catch (ArgumentException)
        {
            return TypedResults.Redirect("/Todo");
        }

        return TypedResults.Redirect("/Todo");
    }
    
    [Route("rename")]
    [HttpPost]
    public Results<BadRequest<string>, RedirectHttpResult> Rename([FromForm] Guid guid, [FromForm] string renameTodo)
    {
        if (string.IsNullOrEmpty(renameTodo))
            return TypedResults.BadRequest("TODO name is invalid.");

        var todo = _todoRepository.GetById(guid);
        if (todo == null)
            return TypedResults.Redirect("/Todo");
        try
        {
            _todoRepository.Update(todo.WithName(renameTodo));
        }
        catch (ArgumentException)
        {
            return TypedResults.Redirect("/Todo");
        }

        return TypedResults.Redirect("/Todo");
    }
}
