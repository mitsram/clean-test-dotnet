using Framework.WebDriver;
using Framework.Interfaces.Adapters;
using NUnit.Framework;

namespace SauceDemo.Tests.Base;

public class BaseTest
{
    protected IWebDriverAdapter driver;    

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // You can set these values based on your test configuration
        // or read them from a configuration file
        WebDriverType webDriverType = WebDriverType.Selenium;
        BrowserType browserType = BrowserType.Chrome;

        driver = WebDriverFactory.Create(webDriverType, browserType);
    }


    [TearDown]
    public void TearDown()
    {
        Thread.Sleep(1000);
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (driver != null)
        {
            driver.Dispose();
        }
    }
}
