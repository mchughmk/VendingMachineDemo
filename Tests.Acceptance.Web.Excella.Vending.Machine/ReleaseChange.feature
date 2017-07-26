Feature: ReleaseChange
	In order to not donate all of my money to the vending machine company
	As a paying customer
	I want to receive change back that isn't used to purchase a product.

Scenario: No change back when none given
	Given I have not inserted a quarter
	When I release the change
	Then I should receive 0 cents in change

Scenario: Get Change Back When not purchasing a product
	Given I have inserted a quarter
	When I release the change
	Then I should receive 25 cents in change

Scenario: Get all my change back when not purchasing
	Given I have inserted a quarter
	And I have inserted a quarter
	And I have inserted a quarter
	When I do not purchase a product
	And I release the change
	Then I should receive 75 cents in change

# Scenario: Get Remaining Change Back when I buy a product
# 	Given I have inserted a quarter
# 	And I have inserted a quarter
# 	And I have inserted a quarter
# 	When I purchase a product
# 	And I release the change
# 	Then I should receive 25 cents in change
