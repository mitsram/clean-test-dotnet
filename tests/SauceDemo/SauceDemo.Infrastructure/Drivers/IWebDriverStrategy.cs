using System;
using System.Collections.Generic;

namespace SauceDemo.Infrastructure.Drivers;

public interface IWebDriverStrategy : IDisposable
{
    void NavigateToUrl(string url);
    IWebElement FindElementById(string id);
    IWebElement FindElementByXPath(string xpath);
    IWebElement FindElementByClassName(string className);
    IReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector);
    IReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath);
    IReadOnlyCollection<IWebElement> FindElementsByClassName(string className);
    string GetCurrentUrl();
}

public interface IWebElement
{
    void SendKeys(string text);
    void Click();
    string Text { get; }
}



