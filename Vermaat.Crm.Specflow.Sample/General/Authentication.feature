Feature: Authentication

@Chrome @API
Scenario: Logging in with a salesperson
	Given a logged in 'Salesperson' named MyUser
	When an account named TestAccount is created with the following values
		| Property     | Value    |
		| Account Name | Checkbox |
	Then TestAccount has the following values
		| Property | Value  |
		| Owner    | MyUser |