

using Framework.WebDriver;
using Microsoft.Extensions.Configuration;

namespace SauceDemo.Tests.Config;

public class TestConfiguration(IConfiguration configuration)
{
    public WebDriverType WebDriverType { get; } = configuration.GetValue<WebDriverType>("WebDriverType", WebDriverType.Selenium);
    public BrowserType BrowserType { get; } = configuration.GetValue<BrowserType>("BrowserType", BrowserType.Chrome);
    public string BaseUrl { get; } = configuration.GetValue<string>("TestSettings:BaseUrl", "https://www.saucedemo.com/");
    public int Timeout { get; } = configuration.GetValue<int>("TestSettings:Timeout", 30);
}

