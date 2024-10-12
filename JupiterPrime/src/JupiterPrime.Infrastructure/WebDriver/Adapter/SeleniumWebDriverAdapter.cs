using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JupiterPrime.Application.Interfaces;

namespace JupiterPrime.Infrastructure.WebDriver
{
    public class SeleniumWebDriverAdapter : IWebDriverAdapter
    {
        private readonly IWebDriver _driver;

        public SeleniumWebDriverAdapter(IWebDriver driver)
        {
            _driver = driver;
        }

        public Task NavigateToAsync(string url)
        {
            _driver.Navigate().GoToUrl(url);
            return Task.CompletedTask;
        }

        public Task<IWebElementAdapter> FindElementAsync(string selector)
        {
            var element = selector.StartsWith("//") 
                ? _driver.FindElement(By.XPath(selector))
                : _driver.FindElement(By.CssSelector(selector));
            return Task.FromResult<IWebElementAdapter>(new SeleniumWebElementAdapter(element));
        }

        public Task<IWebElementAdapter> FindElementAsync(IWebElementAdapter parentElement, string selector)
        {
            var seleniumParentElement = ((SeleniumWebElementAdapter)parentElement).Element;
            var element = selector.StartsWith("//") 
                ? seleniumParentElement.FindElement(By.XPath(selector))
                : seleniumParentElement.FindElement(By.CssSelector(selector));
            return Task.FromResult<IWebElementAdapter>(new SeleniumWebElementAdapter(element));
        }

        public Task<IReadOnlyList<IWebElementAdapter>> FindElementsAsync(string selector)
        {
            var elements = _driver.FindElements(By.CssSelector(selector))
                .Select(e => new SeleniumWebElementAdapter(e) as IWebElementAdapter)
                .ToList();
            return Task.FromResult<IReadOnlyList<IWebElementAdapter>>(elements);
        }

        public Task<string> GetTextAsync(IWebElementAdapter element, string selector)
        {
            var seleniumElement = ((SeleniumWebElementAdapter)element).Element;
            var text = seleniumElement.FindElement(By.CssSelector(selector)).Text;
            return Task.FromResult(text);
        }

        public Task ClickElementAsync(IWebElementAdapter element)
        {
            var seleniumElement = ((SeleniumWebElementAdapter)element).Element;
            seleniumElement.Click();
            return Task.CompletedTask;
        }

        public Task<string> GetCurrentUrlAsync()
        {
            return Task.FromResult(_driver.Url);
        }

        public Task WaitForElementAsync(string selector, int timeoutInSeconds = 30)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(d => d.FindElement(By.CssSelector(selector)).Displayed);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }

    public class SeleniumWebElementAdapter : IWebElementAdapter
    {
        public IWebElement Element { get; }

        public SeleniumWebElementAdapter(IWebElement element)
        {
            Element = element;
        }

        public Task ClickAsync()
        {
            Element.Click();
            return Task.CompletedTask;
        }

        public Task SendKeysAsync(string text)
        {
            Element.SendKeys(text);
            return Task.CompletedTask;
        }

        public Task<string> GetTextAsync()
        {
            return Task.FromResult(Element.Text);
        }

        public Task<bool> IsDisplayedAsync()
        {
            return Task.FromResult(Element.Displayed);
        }

        public Task<bool> IsEnabledAsync()
        {
            return Task.FromResult(Element.Enabled);
        }
    }
}
