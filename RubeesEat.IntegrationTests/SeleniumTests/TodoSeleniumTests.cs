using RubeesEat.IntegrationTests.PageObjects;
using RubeesEat.Model;

namespace RubeesEat.IntegrationTests.SeleniumTests;

public class TodoSeleniumTests : SeleniumIntegrationTestBase
{
    public TodoSeleniumTests()
        : base("Todo")
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
    public void DeleteTodo()
    {
        var page = Start<TodoPageObject>();
        page.GetTodos()[0].ClickDeleteTodo();
        page.ClickConfirmDelete();

        Assert.That(page.GetTodoTexts(), Is.Empty);
    }

    [Test]
    public void CancelDeleteTodo()
    {
        var page = Start<TodoPageObject>();
        page.GetTodos()[0].ClickDeleteTodo();
        page.ClickCancelTodo();

        Assert.That(
            page.GetTodoTexts(),
            Is.EqualTo(new[] { "My first todo" }));
    }

    [Test]
    [Ignore("TODO: DSEAT-7 Add web test test deleting same TODO twice")]
    public void DeleteAlreadyDeletedTodo()
    {
        // TODO: DSEAT-7 Add web test test deleting same TODO twice
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

    [Test]
    public void ConfirmRename()
    {
        var page = Start<TodoPageObject>();
        var todoControlObject = page.GetTodos()[0];
        string expectedName = "New todo name";

        using (var renamePage = todoControlObject.ClickRenameTodo())
        {
            renamePage.SetNewRenameText(expectedName);
            renamePage.ClickConfirmRename();
        }

        todoControlObject = page.GetTodos()[0];
        Assert.That(page, Is.Not.Null);
        Assert.That(todoControlObject.Text, Is.EqualTo(expectedName));
    }

    [Test]
    public void CancelRename()
    {
        var page = Start<TodoPageObject>();
        var renamePage = Start<RenamePageObject>();

        var todoControlObject = page.GetTodos()[0];
        todoControlObject.ClickRenameTodo();
        renamePage.ClickCancelRename();
        Assert.That(
            page.GetTodoTexts(),
            Is.EqualTo(new[] { "My first todo" }));
    }

    [Test]
    public void EmptyStringRename()
    {
        var page = Start<TodoPageObject>();
        var renamePage = Start<RenamePageObject>();

        var todoControlObject = page.GetTodos()[0];
        todoControlObject.ClickRenameTodo();
        string invalidInput = "";
        renamePage.SetNewRenameText(invalidInput);
        renamePage.ClickConfirmRenameNotUntilStale();

        Assert.That(
            page.GetTodoTexts(),
            Is.EqualTo(Array.Empty<string>()));
    }
}
