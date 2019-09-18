Feature: OpportunityTests


@API @Chrome @Cleanup
Scenario Outline: Winning an opportunity
Given an account named TestAccount with the following values
	| Property     | Value       |
	| Account Name | TestAccount |
And a related opportunity from TestAccount named TestOpp with the following values
	| Property      | Value          |
	| Topic         | Test Opp Money |
When the opportunity TestOpp is closed with the following values
	| Property       | Value           |
	| Actual Revenue | 4322            |
	| Status Reason  | <Status Reason> |
Then TestOpp has the following values
| Property      | Value           |
| Topic         | Test Opp Money  |
| Status        | <Status>        |
| Status Reason | <Status Reason> |

Examples:
| Status | Status Reason |
| Won    | Won           |
| Lost   | Canceled      |
| Lost   | Out-Sold      |