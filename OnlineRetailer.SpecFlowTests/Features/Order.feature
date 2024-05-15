Feature: Order

A short summary of the feature


Scenario: User is gathered with credits available
	Given A user is found
	And The user has credits
	When The credit amount is fetched
	Then the credit amount is > 0


@tag1
Scenario Outline: User places multiple orders
	Given A user is created
	And the user has <credits> credits
	And the items total out to <itemTotal>
	When the order is placed
	Then the result is <result>

	Examples: 
	| credits | itemTotal | result |
	| 200 | 100 | True  |
	| 200 | 300 | False |
	| 500 | 500 | True  |
	| 200 | -5  | False |
	| -200| 200 | False |




