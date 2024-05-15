Feature: Customer

A short summary of the feature

@tag1
Scenario: Create a new customer
	Given A repository for customers is created
	When A new customer is created
	Then The customer is inserted into the repository
