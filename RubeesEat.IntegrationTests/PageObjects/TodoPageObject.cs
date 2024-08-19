using OpenQA.Selenium;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public class TodoPageObject : PageObject
{
    public string[] GetTodoTexts()
    {
        return GetTodos()
               .Select(e => e.Text)
               .ToArray();
    }

    public TodoControlObject[] GetTodos()
    {
        return FindElements(By.CssSelector("#todos > li"))
               .Select(e => new TodoControlObject(this, e))
               .ToArray();
    }

    public void SetNewTodoText(string value)
    {
        FindElement(By.Id("newTodoInput")).SendKeys(value);
    }

    public void ClickAddTodo()
    {
        FindElement(By.Id("addTodoButton")).ClickAndWaitUntilStale();
    }

    public void ClickConfirmDelete()
    {
        FindElement(By.CssSelector("#confirmDelete")).ClickAndWaitUntilStale();
    }

    public void ClickCancelTodo()
    {
        FindElement(By.CssSelector("#cancelDelete")).ClickAndWaitUntilStale();
    }
}
