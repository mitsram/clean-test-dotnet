using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JupiterPrime.Application.Interfaces;

namespace JupiterPrime.Infrastructure.WebDriver
{
    public class PlaywrightWebDriverAdapter : IWebDriverAdapter
    {
        private readonly IPage _page;

        public PlaywrightWebDriverAdapter(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToAsync(string url)
        {
            await _page.GotoAsync(url);
        }

        public async Task<IWebElementAdapter> FindElementAsync(string selector)
        {
            var element = selector.StartsWith("//")
                ? await _page.QuerySelectorAsync(selector)
                : await _page.QuerySelectorAsync(selector);
            return new PlaywrightWebElementAdapter(element);
        }

        public async Task<IWebElementAdapter> FindElementAsync(IWebElementAdapter parentElement, string selector)
        {
            var playwrightParentElement = ((PlaywrightWebElementAdapter)parentElement).Element;
            var element = selector.StartsWith("//")
                ? await playwrightParentElement.QuerySelectorAsync(selector)
                : await playwrightParentElement.QuerySelectorAsync(selector);
            return new PlaywrightWebElementAdapter(element);
        }

        public async Task<IReadOnlyList<IWebElementAdapter>> FindElementsAsync(string selector)
        {
            var elements = await _page.QuerySelectorAllAsync(selector);
            return elements.Select(e => new PlaywrightWebElementAdapter(e) as IWebElementAdapter).ToList();
        }

        public async Task<string> GetTextAsync(IWebElementAdapter element, string selector)
        {
            var playwrightElement = ((PlaywrightWebElementAdapter)element).Element;
            var textElement = await playwrightElement.QuerySelectorAsync(selector);
            return await textElement.TextContentAsync();
        }

        public async Task ClickElementAsync(IWebElementAdapter element)
        {
            var playwrightElement = ((PlaywrightWebElementAdapter)element).Element;
            await playwrightElement.ClickAsync();
        }

        public async Task<string> GetCurrentUrlAsync()
        {
            return _page.Url;
        }

        public async Task WaitForElementAsync(string selector, int timeoutInSeconds = 30)
        {
            await _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutInSeconds * 1000
            });
        }

        public void Dispose()
        {
            _page?.CloseAsync().GetAwaiter().GetResult();
        }
    }

    public class PlaywrightWebElementAdapter : IWebElementAdapter
    {
        public IElementHandle Element { get; }

        public PlaywrightWebElementAdapter(IElementHandle element)
        {
            Element = element;
        }

        public async Task ClickAsync()
        {
            await Element.ClickAsync();
        }

        public async Task SendKeysAsync(string text)
        {
            await Element.TypeAsync(text);
        }

        public async Task<string> GetTextAsync()
        {
            return await Element.TextContentAsync();
        }

        public async Task<bool> IsDisplayedAsync()
        {
            return await Element.IsVisibleAsync();
        }

        public async Task<bool> IsEnabledAsync()
        {
            return await Element.IsEnabledAsync();
        }
    }
}
