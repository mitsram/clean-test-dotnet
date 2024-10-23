

using Framework.WebDriver;
using Microsoft.Extensions.Configuration;

namespace SauceDemo.Tests.Config;

public class TestConfiguration
{
    public WebDriverType WebDriverType { get; }
    public BrowserType BrowserType { get; }
    public string BaseUrl { get; }
    public int Timeout { get; }

    public TestConfiguration(IConfiguration configuration)
    {
        WebDriverType = configuration.GetValue<WebDriverType>("WebDriverType", WebDriverType.Selenium);
        BrowserType = configuration.GetValue<BrowserType>("BrowserType", BrowserType.Chrome);
        BaseUrl = configuration.GetValue<string>("TestSettings:BaseUrl", "https://www.saucedemo.com/");
        Timeout = configuration.GetValue<int>("TestSettings:Timeout", 30);
    }
}

