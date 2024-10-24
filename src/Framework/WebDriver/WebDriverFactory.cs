using OpenQA.Selenium;
using Microsoft.Playwright;
using BrowserType = Framework.WebDriver.BrowserType;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Framework.WebDriver.Adapters;
using Framework.WebDriver.Interfaces;

namespace Framework.WebDriver;

public static class WebDriverFactory
{
    public static IWebDriverAdapter Create(WebDriverType webDriverType, BrowserType browserType)
    {
        return webDriverType switch
        {
            WebDriverType.Selenium => new SeleniumWebDriverAdapter(CreateSeleniumDriver(browserType)),
            WebDriverType.Playwright => new PlaywrightWebDriverAdapter(CreatePlaywrightDriver(browserType)),
            _ => throw new ArgumentException("Invalid WebDriverType"),
        };
    }

    private static IWebDriver CreateSeleniumDriver(BrowserType browserType)
    {
        switch (browserType)
        {
            case BrowserType.Chrome:
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--start-maximized");
                return new ChromeDriver(chromeOptions);                
            case BrowserType.Firefox:
                return new FirefoxDriver();
            default:
                throw new ArgumentException("Invalid BrowserType for Selenium");
        }
    }

    private static IPage CreatePlaywrightDriver(BrowserType browserType)
    {
        var browser = CreatePlaywrightBrowser(browserType);
        return browser.NewPageAsync().GetAwaiter().GetResult();
        
    }

    private static IBrowser CreatePlaywrightBrowser(BrowserType browserType)
    {
        var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        switch (browserType)
        {
            case BrowserType.Chrome:
                return playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }).GetAwaiter().GetResult();
            case BrowserType.Firefox:
                return playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }).GetAwaiter().GetResult();
            default:
                throw new ArgumentException("Invalid BrowserType for Playwright");
        }
    }
}



