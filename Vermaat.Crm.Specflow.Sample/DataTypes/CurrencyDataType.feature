Feature: CurrencyDataType

@API @Chrome @Cleanup
Scenario: Filling Money fields
Given an account named TestAccount with the following values
	| Property     | Value       |
	| Account Name | TestAccount |
When a related opportunity from TestAccount named TestOpp is created with the following values
	| Property      | Value          |
	| Topic         | Test Opp Money |
	| Budget Amount | 12345          |
Then TestOpp has the following values
	| Property      | Value          |
	| Topic         | Test Opp Money |
	| Budget Amount | 12345          |
	| Account       | TestAccount    |