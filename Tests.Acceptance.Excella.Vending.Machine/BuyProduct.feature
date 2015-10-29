Feature: BuyProduct
	As a customer
	So I can ensure I can buy a product
	Give me the product after I insert money and buy it

Scenario: Buy a product from the vending machine
	Given I have inserted a quarter 
	When I purchase a product
	Then I should receive the product
