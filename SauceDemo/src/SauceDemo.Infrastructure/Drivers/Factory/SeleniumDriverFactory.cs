using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using System;
using SauceDemo.Infrastructure.Drivers;

namespace SauceDemo.Infrastructure.Drivers.Factory;

public class SeleniumDriverFactory
{
    public enum BrowserType
    {
        Chrome,
        Firefox,
        Safari
    }

    public static (IWebDriverAdapter adapter, IWebDriver webDriver) CreateDriver(BrowserType browserType = BrowserType.Chrome)
    {
        IWebDriver webDriver;

        switch (browserType)
        {
            case BrowserType.Chrome:
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--start-maximized");
                webDriver = new ChromeDriver(chromeOptions);
                break;

            case BrowserType.Firefox:
                var firefoxOptions = new FirefoxOptions();
                firefoxOptions.AddArgument("--start-maximized");
                webDriver = new FirefoxDriver(firefoxOptions);
                break;

            case BrowserType.Safari:
                webDriver = new SafariDriver();
                webDriver.Manage().Window.Maximize();
                break;

            default:
                throw new ArgumentException("Unsupported browser type");
        }

        webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        var adapter = new SeleniumAdapter(webDriver);
        return (adapter, webDriver);
    }
}
