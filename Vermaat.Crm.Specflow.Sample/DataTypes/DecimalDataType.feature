Feature: DecimalDataType

@API @Chrome @Cleanup @Set1
Scenario Outline: Filling Decimal fields
Given the current logged in user's settings named CurrentUserSettings
And an account named TestAccount with the following values
	| Property     | Value       |
	| Account Name | TestAccount |
When CurrentUserSettings is updated with the following values
	| Property           | Value  |
	| negativeformatcode | <Code> |
When a related opportunity from TestAccount named TestOpp is created with the following values
	| Property                 | Value          |
	| Topic                    | Test Opp Money |
	| Opportunity Discount (%) | 12.67          |
Then TestOpp has the following values
	| Property                 | Value          |
	| Topic                    | Test Opp Money |
	| Opportunity Discount (%) | 12.67          |
	| parentaccountid          | TestAccount    |

Examples:
| Code |
| 0    |
| 1    |
| 2    |
| 3    |
| 4    |