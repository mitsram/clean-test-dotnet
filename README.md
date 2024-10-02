# clean-test-dotnet

This project is an automation testing framework implementing clean architecture principles and utilizing SpecFlow for behavior-driven development (BDD).

## Table of Contents

1. [Project Structure](#project-structure)
2. [Technologies Used](#technologies-used)
3. [Getting Started](#getting-started)
4. [Running Tests](#running-tests)
5. [Switching Between Playwright and Selenium](#switching-between-playwright-and-selenium)
6. [Writing Tests](#writing-tests)
7. [Reporting](#reporting)
8. [Contributing](#contributing)
9. [License](#license)

## Project Structure

The solution is organized into the following projects:

- `SauceDemo.Domain`: Contains domain entities and interfaces
- `SauceDemo.Infrastructure`: Implements services and drivers
- `SauceDemo.UseCases`: Contains business logic and use cases
- `SauceDemo.Tests`: NUnit-based unit and integration tests
- `SauceDemo.Specflow`: SpecFlow-based BDD tests

## Technologies Used

- .NET 8.0
- NUnit
- SpecFlow
- Selenium WebDriver
- Playwright
- Microsoft.Playwright.NUnit

## Getting Started

1. Clone the repository:
   ```
   git clone git@github.com:mitsram/clean-test-dotnet.git
   ```

2. Install the required .NET SDK (version 8.0 or later).

3. Restore NuGet packages:
   ```
   dotnet restore
   ```

4. Install Playwright browsers:
   ```
   pwsh bin/Debug/net8.0/playwright.ps1 install
   ```

## Running Tests

To run all tests:

```
dotnet test
```

To run specific test projects:

```
dotnet test tests/SauceDemo/SauceDemo.Tests
dotnet test tests/SauceDemo/SauceDemo.Specflow
```

## Switching Between Playwright and Selenium

This framework supports both Playwright and Selenium WebDriver for web automation. You can easily switch between the two by modifying a single enum value in the `BaseTest` class.

### Configuration

1. Open the `BaseTest.cs` file in the `SauceDemo.Tests` project:

```csharp:SauceDemo/tests/SauceDemo.Tests/BaseTest.cs
public class BaseTest
{
    // ... other code ...

    protected enum DriverType
    {
        Selenium,
        Playwright
    }

    // Set the desired driver type here
    protected DriverType CurrentDriverType = DriverType.Playwright;

    // ... rest of the class ...
}
```

2. To switch between Playwright and Selenium, simply change the `CurrentDriverType` value:

   - For Playwright: `protected DriverType CurrentDriverType = DriverType.Playwright;`
   - For Selenium: `protected DriverType CurrentDriverType = DriverType.Selenium;`

### Driver Initialization

The framework automatically initializes the appropriate driver based on the `CurrentDriverType` value in the `InitializeDriver` method:

```csharp:SauceDemo/tests/SauceDemo.Tests/BaseTest.cs
private async Task InitializeDriver()
{
    switch (CurrentDriverType)
    {
        case DriverType.Selenium:
            IWebDriver webdriver = new ChromeDriver();
            webdriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            webdriver.Manage().Window.Maximize();
            driver = new SeleniumWebDriver(webdriver);
            break;

        case DriverType.Playwright:
            (_playwright, _browser, _page, _context, driver) = await PlaywrightDriverFactory.CreateDriverAsync();
            break;

        default:
            throw new ArgumentException("Invalid driver type specified");
    }
}
```

### Usage in Tests

When writing tests, use the `driver` field to interact with the browser. This abstraction allows your tests to work with either Playwright or Selenium without modification:

```csharp:SauceDemo/tests/SauceDemo.Tests/SampleTest.cs
[Test]
public async Task SampleTest()
{
    await driver.NavigateToAsync("https://www.saucedemo.com");
    // Perform test actions using the driver
}
```

### Teardown

The framework handles the proper disposal of resources for both Playwright and Selenium in the `TearDown` method:

```csharp:SauceDemo/tests/SauceDemo.Tests/BaseTest.cs
public virtual async Task TearDown()
{
    try
    {
        if (CurrentDriverType == DriverType.Playwright)
        {
            string testName = TestContext.CurrentContext.Test.Name;
            if (_context?.Tracing != null)
            {
                await _context.Tracing.StopAsync(new()
                {
                    Path = $"trace-{testName}.zip"
                });
            }

            await _page!.CloseAsync();
            await _browser!.CloseAsync();
            _playwright?.Dispose();                
        }
        else
        {
            driver?.Dispose();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during driver disposal: {ex.Message}");
    }

    // rest of the class ...
}
```

By following this approach, you can easily switch between Playwright and Selenium while keeping most of your test code unchanged. The framework handles the initialization and teardown processes automatically based on the selected driver type.

## Writing Tests

### NUnit Tests

Add new test classes to the `SauceDemo.Tests` project. Example:

```csharp
[TestFixture]
public class LoginTests : BaseTest
{
    [Test]
    public void Should_LoginSuccessfully_WhenCredentialsAreValid()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

### BDD Tests (SpecFlow)

1. Add new feature files to the `SauceDemo.Specflow/Features` directory.
2. Implement step definitions in the `SauceDemo.Specflow/StepDefinitions` directory.

Example Feature:

```gherkin
Feature: Login
    As a user of the Sauce Demo website
    I want to be able to log in
    So that I can access the inventory page

Scenario: Successful login with valid credentials
    Given I am on the login page
    When I enter valid credentials
    Then I should be logged in successfully
```

## Reporting

The framework generates Playwright HTML reports after test execution. You can find these reports in the `playwright-report` directory.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

