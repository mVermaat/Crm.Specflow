Feature: NumericFormulas


@API @Cleanup @Set1
Scenario: Filling Decimal fields
Given an account named TestAccount with the following values
	| Property     | Value       |
	| Account Name | TestAccount |
When a related opportunity from TestAccount named TestOpp is created with the following values
	| Property                 | Value          |
	| Topic                    | Test Opp Money |
	| Opportunity Discount (%) | =10.2 + 10.25  |
Then TestOpp has the following values
	| Property                 | Value          |
	| Topic                    | Test Opp Money |
	| Opportunity Discount (%) | 20.45          |
	| parentaccountid          | TestAccount    |
