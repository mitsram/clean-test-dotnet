using JupiterPrime.Application.Interfaces;
using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace JupiterPrime.Infrastructure.WebDriver
{
    public class WebDriverFactory : IWebDriverFactory, IDisposable
    {
        private readonly WebDriverType _webDriverType;
        private IWebDriverAdapter _currentDriver;
        private IPlaywright _playwright;
        private IBrowser _browser;

        public WebDriverFactory(WebDriverType webDriverType)
        {
            _webDriverType = webDriverType;
        }

        public IWebDriverAdapter CreateWebDriver()
        {
            switch (_webDriverType)
            {
                case WebDriverType.Selenium:
                    _currentDriver = CreateSeleniumWebDriver();
                    return _currentDriver;
                case WebDriverType.Playwright:
                    _currentDriver = CreatePlaywrightWebDriver();
                    return _currentDriver;
                default:
                    throw new ArgumentException("Invalid WebDriver type");
            }
        }

        private IWebDriverAdapter CreateSeleniumWebDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            var driver = new ChromeDriver(options);
            return new SeleniumWebDriverAdapter(driver);
        }

        private IWebDriverAdapter CreatePlaywrightWebDriver()
        {
            _playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
            _browser = _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            }).GetAwaiter().GetResult();
            var page = _browser.NewPageAsync().GetAwaiter().GetResult();
            return new PlaywrightWebDriverAdapter(page);
        }

        public void Dispose()
        {
            _currentDriver?.Dispose();
            _browser?.DisposeAsync().AsTask().GetAwaiter().GetResult();
            _playwright?.Dispose();
        }
    }

    public enum WebDriverType
    {
        Selenium,
        Playwright
    }
}
