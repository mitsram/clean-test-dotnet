Feature: Login
    As a user of the Sauce Demo website
    I want to be able to log in
    So that I can access the inventory page

Scenario Outline: Login with various credentials
    Given I am on the login page
    When I attempt to login with username "<username>" and password "<password>"
    Then the login result should be "<expectedResult>"
    And I should be on the "<expectedPage>" page

Examples:
    | username        | password     | expectedResult | expectedPage |
    | standard_user   | secret_sauce | successful     | inventory    |
    | locked_out_user | secret_sauce | unsuccessful   | login        |
    | invalid_user    | invalid_password | unsuccessful | login      |
