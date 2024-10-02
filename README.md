# clean-test-dotnet

This project is an automation testing framework implementing clean architecture principles and utilizing SpecFlow for behavior-driven development (BDD).

## Table of Contents

1. [Project Structure](#project-structure)
2. [Technologies Used](#technologies-used)
3. [Getting Started](#getting-started)
4. [Running Tests](#running-tests)
5. [Framework Architecture](#framework-architecture)
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

## Framework Architecture

This project follows clean architecture principles:

1. **Domain Layer**: Contains core business logic and entities.
2. **Use Cases Layer**: Implements application-specific business rules.
3. **Infrastructure Layer**: Provides implementations for external services and tools.
4. **Presentation Layer**: In this case, our test projects act as the presentation layer.

## Writing Tests

### Unit Tests (NUnit)

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