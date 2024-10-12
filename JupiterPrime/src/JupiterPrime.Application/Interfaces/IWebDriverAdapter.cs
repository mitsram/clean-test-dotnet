using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JupiterPrime.Application.Interfaces;

public interface IWebDriverAdapter : IDisposable
{
    Task NavigateToAsync(string url);
    Task<IWebElementAdapter> FindElementAsync(string selector);
    Task<IWebElementAdapter> FindElementAsync(IWebElementAdapter parentElement, string selector);
    Task<IReadOnlyList<IWebElementAdapter>> FindElementsAsync(string selector);
    Task<string> GetTextAsync(IWebElementAdapter element, string selector);
    Task ClickElementAsync(IWebElementAdapter element);
    Task<string> GetCurrentUrlAsync();
    Task WaitForElementAsync(string selector, int timeoutInSeconds = 30);
}
