using Microsoft.Playwright;
using StrategyElement = SauceDemo.Infrastructure.Drivers.IWebElement;

namespace SauceDemo.Infrastructure.Drivers;

public class PlaywrightWebDriverStrategy : IWebDriverStrategy
{
    private readonly IPage _page;

    public PlaywrightWebDriverStrategy(IPage page)
    {
        _page = page;
    }

    public void NavigateToUrl(string url) => _page.GotoAsync(url).GetAwaiter().GetResult();

    public StrategyElement FindElementById(string id) => 
        new PlaywrightWebElementAdapter(_page.Locator($"#{id}"));

    public StrategyElement FindElementByXPath(string xpath) =>
        new PlaywrightWebElementAdapter(_page.Locator(xpath));

    public StrategyElement FindElementByClassName(string className) =>
        new PlaywrightWebElementAdapter(_page.Locator($".{className}"));

    public IReadOnlyCollection<StrategyElement> FindElementsByCssSelector(string cssSelector) =>
        _page.QuerySelectorAllAsync(cssSelector).GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    public IReadOnlyCollection<StrategyElement> FindElementsByXPath(string xpath) =>
        _page.QuerySelectorAllAsync(xpath).GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    public IReadOnlyCollection<StrategyElement> FindElementsByClassName(string className) =>
        _page.QuerySelectorAllAsync($".{className}").GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    public string GetCurrentUrl() => _page.Url;

    public void Dispose() => _page.CloseAsync().GetAwaiter().GetResult();
}

public class PlaywrightWebElementAdapter : StrategyElement
{
    private readonly ILocator _element;

    public PlaywrightWebElementAdapter(ILocator element)
    {
        _element = element;
    }

    public void SendKeys(string text) => _element.FillAsync(text).GetAwaiter().GetResult();
    public void Click() => _element.ClickAsync().GetAwaiter().GetResult();
    public string Text => _element.TextContentAsync().GetAwaiter().GetResult();
}

public class PlaywrightElementHandleAdapter : StrategyElement
{
    private readonly IElementHandle _element;

    public PlaywrightElementHandleAdapter(IElementHandle element)
    {
        _element = element;
    }

    public void SendKeys(string text) => _element.FillAsync(text).GetAwaiter().GetResult();
    public void Click() => _element.ClickAsync().GetAwaiter().GetResult();
    public string Text => _element.TextContentAsync().GetAwaiter().GetResult();
}
