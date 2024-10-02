Feature: Shop
  As a standard user
  I want to manage products in my cart and sort products
  So that I can have a smooth shopping experience

Background:
  Given I am logged in as a standard user

Scenario: Add product to cart
  When I add "Sauce Labs Backpack" to my cart
  Then "Sauce Labs Backpack" should be in my cart
  And the cart item count should be 1

Scenario: Remove product from cart
  Given I have added "Sauce Labs Bike Light" to my cart
  When I remove "Sauce Labs Bike Light" from my cart
  Then "Sauce Labs Bike Light" should not be in my cart
  And the cart item count should be 0

Scenario: Sort products
  When I sort products by "Price (high to low)"
  Then the products should be sorted correctly by "Price (high to low)"
