using System.Net;
using Newtonsoft.Json;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests.Controllers;

[TestFixture]
public class TodoControllerTests : IntegrationTestBase
{
    [Test]
    public async Task GetAll()
    {
        var result = await HttpClient.GetStringAsync("/api/todos");

        var todos = JsonConvert.DeserializeObject<TodoItem[]>(result);
        Assert.That(todos, Is.Not.Null);
        Assert.That(todos, Has.Length.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(todos[0].Name, Is.EqualTo("My first todo"));
            Assert.That(todos[0].IsCompleted, Is.False);
        });
    }

    [Test]
    public async Task Add()
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("name", "My new Todo")
        });

        var response = await HttpClient.PostAsync("/api/todos", content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));

        var result = await HttpClient.GetStringAsync("/api/todos");

        var todos = JsonConvert.DeserializeObject<TodoItem[]>(result);
        Assert.That(todos, Is.Not.Null);
        Assert.That(todos, Has.Length.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(todos[1].Name, Is.EqualTo("My new Todo"));
            Assert.That(todos[1].IsCompleted, Is.False);
        });
    }

    [Test]
    public async Task Delete()
    {
        var result = await HttpClient.GetStringAsync("/api/todos");
        var todos = JsonConvert.DeserializeObject<TodoItem[]>(result);
        var url = "/api/todos/delete";
        var content = new FormUrlEncodedContent(
            new[]
            {
                new KeyValuePair<string, string>("guid", todos[0].Id.ToString())
            });

        var response = await HttpClient.PostAsync(url, content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));

        var resultAfter = await HttpClient.GetStringAsync("/api/todos");
        var todosAfter = JsonConvert.DeserializeObject<TodoItem[]>(resultAfter);
        Assert.That(todosAfter, Has.Length.EqualTo(0));
    }

    [Test]
    public async Task DeleteInvalidID()
    {
        var result = await HttpClient.GetStringAsync("/api/todos");
        var todos = JsonConvert.DeserializeObject<TodoItem[]>(result);
        var url = "/api/todos/delete";
        var content = new FormUrlEncodedContent(
            new[]
            {
                new KeyValuePair<string, string>("guid", "6e7d8ae1-0a01-4a6e-9c22-95f71443f1f2")
            });

        var response = await HttpClient.PostAsync(url, content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
    }

    [Test]
    public async Task Checkbox()
    {
        var result = await HttpClient.GetStringAsync("/api/todos");
        var todos = JsonConvert.DeserializeObject<TodoItem[]>(result);
        var url = "/api/todos/checkbox";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("guid", todos[0].Id.ToString()),
            new KeyValuePair<string, string>("checkbox", "true")
        });

        var response = await HttpClient.PostAsync(url, content);
        var resultAfter = await HttpClient.GetStringAsync("/api/todos");
        var todosAfter = JsonConvert.DeserializeObject<TodoItem[]>(resultAfter);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
        Assert.That(todosAfter[0].IsCompleted, Is.EqualTo(true));
    }

    [Test]
    public async Task CheckboxInvalidID()
    {
        var url = "/api/todos/checkbox";
        var content = new FormUrlEncodedContent(
            new[]
            {
                new KeyValuePair<string, string>("guid", "6e7d8ae1-0a01-4a6e-9c22-95f71443f1f2")
            });

        var response = await HttpClient.PostAsync(url, content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Rename()
    {
        var result = await HttpClient.GetStringAsync("/api/todos");
        var todos = JsonConvert.DeserializeObject<TodoItem[]>(result);
        var content = new FormUrlEncodedContent(
            new[]
            {
                new KeyValuePair<string, string>("renameTodo", "Renamed"),
                new KeyValuePair<string, string>("guid", todos[0].Id.ToString())
            });
        var response = await HttpClient.PostAsync("/api/todos/rename", content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
        result = await HttpClient.GetStringAsync("/api/todos");

        var todos2 = JsonConvert.DeserializeObject<TodoItem[]>(result);
        Assert.That(todos2, Is.Not.Null);
        Assert.That(todos2, Has.Length.EqualTo(1));
        Assert.Multiple(
            () =>
            {
                Assert.That(todos2[0].Name, Is.EqualTo("Renamed"));
                Assert.That(todos2[0].IsCompleted, Is.False);
            });
    }

    [Test]
    public async Task RenameWithEmptyString()
    {
        var result = await HttpClient.GetStringAsync("/api/todos");
        var todos = JsonConvert.DeserializeObject<TodoItem[]>(result);
        var content = new FormUrlEncodedContent(
            new[]
            {
                new KeyValuePair<string, string>("renameTodo", ""),
                new KeyValuePair<string, string>("guid", todos[0].Id.ToString())
            });
        var response = await HttpClient.PostAsync("/api/todos/rename", content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task RenameWithInvalidID()
    {
        var result = await HttpClient.GetStringAsync("/api/todos");
        var todos = JsonConvert.DeserializeObject<TodoItem[]>(result);
        var content = new FormUrlEncodedContent(
            new[]
            {
                new KeyValuePair<string, string>("renameTodo", "asrg"),
                new KeyValuePair<string, string>("guid", "582adda4-e1b7-4a03-b5d3-358adb42105e")
            });
        var response = await HttpClient.PostAsync("/api/todos/rename", content);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));

        var result2 = await HttpClient.GetStringAsync("/api/todos");
        var todos2 = JsonConvert.DeserializeObject<TodoItem[]>(result2);
        Assert.That(todos, Is.EqualTo(todos2));
    }
}
