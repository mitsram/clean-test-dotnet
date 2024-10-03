Feature: Shop
    As a user
    I want to be able to add and remove products from my cart
    So that I can manage my shopping experience

Background:
    Given I am logged in as a standard user

Scenario: Add product to cart
    Given I am on the products page
    When I add the "Sauce Labs Backpack" to the cart
    Then the "Sauce Labs Backpack" should be in the cart
    And the cart item count should be 1

Scenario: Remove product from cart
    Given I am on the products page
    And I have added the "Sauce Labs Bike Light" to the cart
    When I remove the "Sauce Labs Bike Light" from the cart
    Then the "Sauce Labs Bike Light" should not be in the cart
    And the cart item count should be 0

Scenario: Sort products
    Given I am on the products page
    When I sort the products by "Price (high to low)"
    Then the products should be sorted correctly by "Price (high to low)"
