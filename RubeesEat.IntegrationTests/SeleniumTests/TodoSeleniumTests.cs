using RubeesEat.IntegrationTests.PageObjects;

namespace RubeesEat.IntegrationTests.SeleniumTests;

public class TodoSeleniumTests : SeleniumIntegrationTestBase
{
    public TodoSeleniumTests() : base("Todo")
    {
    }

    [Test]
    public void GetDefaultTodos()
    {
        var page = Start<TodoPageObject>();

        Assert.That(
            page.GetTodoTexts(),
            Is.EqualTo(new[] { "My first todo" }));
    }

    [Test]
    public void AddTodo()
    {
        var page = Start<TodoPageObject>();

        page.SetNewTodoText("My new todo");
        page.ClickAddTodo();

        Assert.That(
            page.GetTodoTexts(),
            Is.EqualTo(new[] { "My first todo", "My new todo" }));
    }

    [Test]
    public void CheckTodo()
    {
        TodoPageObject page = Start<TodoPageObject>();
        var todos = page.GetTodos();
        todos[0].ToggleChecked();

        page.Refresh();
        todos = page.GetTodos();

        Assert.That(
            todos[0].IsChecked,
            Is.EqualTo((true))
        );
    }

    [Test]
    public void CheckTodoSecondClick()
    {
        var page = Start<TodoPageObject>();
        var todos = page.GetTodos();
        todos[0].ToggleChecked();

        page.Refresh();
        todos = page.GetTodos();

        Assert.That(
            todos[0].IsChecked,
            Is.EqualTo((true))
        );

        todos[0].ToggleChecked();
        page.Refresh();
        todos = page.GetTodos();

        Assert.That(
            todos[0].IsChecked,
            Is.EqualTo((false))
        );
    }
}
