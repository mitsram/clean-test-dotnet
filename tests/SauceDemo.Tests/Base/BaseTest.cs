using Framework.WebDriver;
using Framework.Interfaces.Adapters;
using NUnit.Framework;

namespace SauceDemo.Tests.Base;

public class BaseTest : IAsyncDisposable
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
        public virtual async Task TearDown()
        {
            await DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (driver != null)
            {
                await driver.DisposeAsync();
            }
        }
}
