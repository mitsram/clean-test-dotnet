namespace Framework.Interfaces.Adapters;

public interface IWebDriverAdapter : IAsyncDisposable
{
    void NavigateToUrl(string url);
    string GetCurrentUrl();
    IWebElementAdapter FindElementById(string id);
    IWebElementAdapter FindElementByXPath(string xpath);
    IWebElementAdapter FindElementByClassName(string className);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByCssSelector(string cssSelector);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByXPath(string xpath);
    IReadOnlyCollection<IWebElementAdapter> FindElementsByClassName(string className);
    
}