Feature: Purchase
  As a customer of the Sauce Demo store
  I want to be able to purchase products
  So that I can complete my shopping experience

Scenario: Complete a purchase with valid details
  Given I am logged in as a standard user
  And I have added "Sauce Labs Backpack" to my cart
  And I have added "Sauce Labs Bike Light" to my cart
  When I proceed to checkout
  And I fill in the following customer information:
    | FirstName | LastName | ZipCode |
    | John      | Doe      | 12345   |
  And I continue to the overview page
  And I complete the purchase
  Then the purchase should be successful
  And I should be on the order complete page
  And I should see the confirmation message "Thank you for your order!"
