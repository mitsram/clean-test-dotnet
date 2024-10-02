using OpenQA.Selenium;
using SeleniumElement = OpenQA.Selenium.IWebElement;
using StrategyElement = SauceDemo.Infrastructure.Drivers.IWebElement;

namespace SauceDemo.Infrastructure.Drivers;

public class SeleniumWebDriverStrategy : IWebDriverStrategy
{
    private readonly IWebDriver _driver;

    public SeleniumWebDriverStrategy(IWebDriver driver)
    {
        _driver = driver;
    }

    public void NavigateToUrl(string url) => _driver.Navigate().GoToUrl(url);

    public StrategyElement FindElementById(string id) => 
        new SeleniumWebElementAdapter(_driver.FindElement(By.Id(id)));

    public StrategyElement FindElementByXPath(string xpath) =>
        new SeleniumWebElementAdapter(_driver.FindElement(By.XPath(xpath)));

    public StrategyElement FindElementByClassName(string className) =>
        new SeleniumWebElementAdapter(_driver.FindElement(By.ClassName(className)));

    public IReadOnlyCollection<StrategyElement> FindElementsByCssSelector(string cssSelector) =>
        _driver.FindElements(By.CssSelector(cssSelector))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();

    public IReadOnlyCollection<StrategyElement> FindElementsByXPath(string xpath) =>
        _driver.FindElements(By.XPath(xpath))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();

    public IReadOnlyCollection<StrategyElement> FindElementsByClassName(string className) =>
        _driver.FindElements(By.ClassName(className))
            .Select(e => new SeleniumWebElementAdapter(e))
            .ToList();

    public string GetCurrentUrl() => _driver.Url;

    public void Dispose() => _driver.Quit();
}

public class SeleniumWebElementAdapter : StrategyElement
{
    private readonly SeleniumElement _element;

    public SeleniumWebElementAdapter(SeleniumElement element)
    {
        _element = element;
    }

    public void SendKeys(string text) => _element.SendKeys(text);
    public void Click() => _element.Click();
    public string Text => _element.Text;
}
