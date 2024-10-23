using Framework.ApiDriver.Adapters;
using Framework.ApiDriver.Interfaces;

namespace PetApi.Tests
{
    public class BaseTest
    {
        protected IApiDriverAdapter ApiDriver { get; private set; }
        protected const string BaseUrl = "https://petstore.swagger.io/v2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Global setup code, if any
            Console.WriteLine("Starting test suite execution...");
        }

        [SetUp]
        public void SetUp()
        {
            // Choose the API client adapter based on configuration or environment variable
            string apiClientType = "HttpClient"; // Environment.GetEnvironmentVariable("API_CLIENT_TYPE") ?? "HttpClient";

            ApiDriver = apiClientType.ToLower() switch
            {
                "httpclient" => new HttpClientAdapter(BaseUrl),
                "restsharp" => new RestSharpAdapter(BaseUrl),
                _ => throw new ArgumentException($"Unsupported API client type: {apiClientType}")
            };

            Console.WriteLine($"Using {apiClientType} for API communication");
        }

        [TearDown]
        public void TearDown()
        {
            // Common teardown code, if any
            Console.WriteLine("Test completed.");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Global teardown code, if any
            Console.WriteLine("Test suite execution completed.");
        }

        protected void LogTestInfo(string message)
        {
            Console.WriteLine($"[{TestContext.CurrentContext.Test.Name}] {message}");
        }
    }
}
