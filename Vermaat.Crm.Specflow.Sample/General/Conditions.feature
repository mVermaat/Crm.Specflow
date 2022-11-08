Feature: Conditions

You use use conditions like these :
https://learn.microsoft.com/en-us/power-apps/developer/data-platform/webapi/reference/conditionoperator?view=dataverse-latest#members

@API @Set2 @Cleanup
Scenario: Getting records with conditions
Given an account named TestAccount with the following values
	| Property     | Value                   |
	| Account Name | TestAccount             |
	| Website      | https://somewebsite.com |
	| Credit Limit | 50000000                |
	| Industry     | Financial               |
Then an account exists with the following values
	| Property     | Condition   | Value         |
	| Account Name | Equal       | TestAccount   |
	| Website      | Like        | %somewebsite% |
	| Credit Limit | GreaterThan | 1000000       |
	| SIC Code     | Null        |               |
	| Industry     | NotNull     |               |

