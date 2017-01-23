Feature: BuyProduct
	As a customer
	So I can ensure I can buy a product
	Give me the product after I insert money and buy it

Scenario: Balance is Updated on Coin Insert
	When I insert a Quarter
	Then The balance should be 25 cents

Scenario: Insert Multiple Coins
	When I insert a Quarter
	And I insert a Quarter
	And I insert a Quarter
	Then The balance should be 75 cents

# Scenario: Buy a product from the vending machine
# 	Given I have inserted a quarter 
# 	And I have inserted a quarter
# 	When I purchase a product
# 	Then I should receive the product

# Scenario: I cannot buy a product if I don't insert money
# 	Given I have not inserted a quarter
# 	When I purchase a product
# 	Then I should not receive a product

# Scenario: I cannot buy a product with only one quarter
# 	Given I have inserted a quarter
# 	When I purchase a product
# 	Then I should not receive a product
