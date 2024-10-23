using Framework.Interfaces.Adapters;
using Microsoft.Playwright;


namespace Framework.WebDriver.Adapters;

public class PlaywrightWebDriverAdapter : IWebDriverAdapter
{
    private readonly IPage _page;

    public PlaywrightWebDriverAdapter(IPage page)
    {
        _page = page;
    }

    public void NavigateToUrl(string url) => _page.GotoAsync(url).GetAwaiter().GetResult();

    public IWebElementAdapter FindElementById(string id) => 
        new PlaywrightWebElementAdapter(_page.Locator($"#{id}"));

    public IWebElementAdapter FindElementByXPath(string xpath) =>
        new PlaywrightWebElementAdapter(_page.Locator(xpath));

    public IWebElementAdapter FindElementByClassName(string className) =>
        new PlaywrightWebElementAdapter(_page.Locator($".{className}"));

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector) =>
        _page.QuerySelectorAllAsync(cssSelector).GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath) =>
        _page.QuerySelectorAllAsync(xpath).GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    public IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className) =>
        _page.QuerySelectorAllAsync($".{className}").GetAwaiter().GetResult()
            .Select(e => new PlaywrightElementHandleAdapter(e))
            .ToList();

    public string GetCurrentUrl() => _page.Url;

    public void Dispose() => _page.CloseAsync().GetAwaiter().GetResult();
}

public class PlaywrightWebElementAdapter : IWebElementAdapter
{
    private readonly ILocator _element;

    public PlaywrightWebElementAdapter(ILocator element)
    {
        _element = element;
    }

    public void SendKeys(string text) => _element.FillAsync(text).GetAwaiter().GetResult();
    public void Click() => _element.ClickAsync().GetAwaiter().GetResult();

    public string GetText()
    {
        throw new NotImplementedException();
    }

    public string Text => _element.TextContentAsync().GetAwaiter().GetResult() ?? string.Empty;
}

public class PlaywrightElementHandleAdapter : IWebElementAdapter
{
    private readonly IElementHandle _element;

    public PlaywrightElementHandleAdapter(IElementHandle element)
    {
        _element = element;
    }

    public void SendKeys(string text) => _element.FillAsync(text).GetAwaiter().GetResult();
    public void Click() => _element.ClickAsync().GetAwaiter().GetResult();

    public string GetText()
    {
        throw new NotImplementedException();
    }

    public string Text => _element.TextContentAsync().GetAwaiter().GetResult() ?? string.Empty;
}
