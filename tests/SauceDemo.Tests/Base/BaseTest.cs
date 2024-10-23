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
    
    [OneTimeTearDown]
    public void TearDown()
    {
        if (driver != null)
        {
            driver.Dispose();
        }
    }
}
