Feature: Product

A short summary of the feature

@tag1
Scenario: Check availability of product
	Given A product is created
	And There are <quantity> items available
	When the status is checked
	Then the availability is <result>

	Examples: 
	| quantity | result |
	| 3 | true|
	| 0 | false |
	| -1 | false |
	| 100| true |
