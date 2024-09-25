using System.Collections.ObjectModel;
using OpenQA.Selenium;
using RubeesEat.Exceptions;
using RubeesEat.IntegrationTests.WebTesting;

namespace RubeesEat.IntegrationTests.PageObjects;

public static class WebTestElementFinder
{
    public static IWebElement FindElement(ISearchContext context, string id)
    {
        ReadOnlyCollection<IWebElement> elements = context.FindElements(By.CssSelector($"[data-test-id='{id}']"));
        if (elements.Count == 0)
            throw new NoSuchElementException($"Element with test id: '{id}' was not found.");

        if (elements.Count > 1)
            throw new TooManyElementsException($"Element with test id: '{id}' returned more than one element.");

        return elements.First();
    }
    
    public static IReadOnlyCollection<IWebElement> FindElements(ISearchContext context, string id)
    {
        ReadOnlyCollection<IWebElement> elements = context.FindElements(By.CssSelector($"[data-test-id='{id}']"));
        if (elements.Count == 0)
            throw new NoSuchElementException($"Element with test id: '{id}' was not found.");
        return elements;
    }
    
    public static void InvokeAction(ISearchContext context, string action)
    {
        ReadOnlyCollection<IWebElement> elements = context.FindElements(By.CssSelector($"[data-test-action='{action}']"));
        if (elements.Count == 0)
            throw new NoSuchElementException($"Element with test action: '{action}' was not found.");
        if (elements.Count > 1)
            throw new TooManyElementsException($"Element with test action: '{action}' returned more than one element.");

        var element = elements.First();
        var clickBehavior = element.GetAttribute("data-test-click-behavior");
        switch (clickBehavior)
        {
            case "ClickAndWaitUntilStale":
                element.ClickAndWaitUntilStale();
                break;
            case "Click":
                element.Click();
                break;
            default:
                throw new InvalidOperationException($"Unsupported click behavior '{clickBehavior}' for element with action '{action}'.");
        }
    }
}
