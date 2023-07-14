Feature: Authentication

@Chrome @API @Cleanup @Set1
Scenario: Logging in with a salesperson
	Given a logged in 'Salesperson' named MyUser
	When an account named TestAccount is created with the following values
		| Property     | Value    |
		| Account Name | Checkbox |
	Then TestAccount has the following values
		| Property | Value  |
		| Owner    | MyUser |


@Chrome @Cleanup @Set1
Scenario: Verifying permissions of users
	Given an account named QuoteTesting with the following values
		| Property     | Value        |
		| Account Name | QuoteTesting |
	And a quote named TestQuote with the following values
		| Property           | Value                |
		| Name               | AT Quote to Revise   |
		| Price List         | Automated Testing PL |
		| Potential Customer | QuoteTesting         |
	Then TestQuote has the following user permissions
		| User        | Permissions                    |
		| Salesperson | Read, Write, Append, Append To |
		| Manager     |                                |
