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
}
