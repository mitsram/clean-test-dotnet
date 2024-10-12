using JupiterPrime.Application.UseCases;
using JupiterPrime.Infrastructure.Services;
using JupiterPrime.Infrastructure.WebDriver;
using JupiterPrime.Infrastructure.Configuration;
using NUnit.Framework;
using System;
using Microsoft.Extensions.Configuration;

namespace JupiterPrime.Tests
{
    public abstract class BaseTest : IDisposable
    {
        protected ProductUseCases ProductUseCases { get; private set; }
        private WebDriverFactory _webDriverFactory;
        protected TestConfiguration TestConfig { get; private set; }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            TestConfig = new TestConfiguration(configuration);
        }

        [SetUp]
        public virtual void Setup()
        {
            _webDriverFactory = new WebDriverFactory(TestConfig.WebDriverType);
            var productService = new ProductService(_webDriverFactory, TestConfig.BaseUrl);
            ProductUseCases = new ProductUseCases(productService);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            ProductUseCases?.Dispose();
            _webDriverFactory?.Dispose();
        }
    }
}
