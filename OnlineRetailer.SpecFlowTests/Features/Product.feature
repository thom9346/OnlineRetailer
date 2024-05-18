Feature: Product

A short summary of the feature

@tag1
Scenario: Check availability of product
	Given A product is created
	And There are <AvailableQuantity> items available
	And The customer wants <RequiredQuantity> items
	When the status is checked
	Then the availability is <result>

	Examples: 
	| AvailableQuantity |RequiredQuantity | result |
	| 3 | 2 |true|
	| 0 | 5 | false |
	| -1 | 1 | false |
	| 100| 100 | true |
