Feature: UpdateData

@API @Chrome @Cleanup
Scenario: Update an existing Account
Given an account named TestAccount with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Description  | Test multi line         |
When TestAccount is updated with the following values
	| Property     | Value             |
	| Account Name | DynamicHands B.V. |
	| Main Phone   | 06987654321       |
	| Fax          | 4839432324        |
	| Description  | Update multi line |
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands B.V.       |
	| Main Phone   | 06987654321             |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Description  | Update multi line       |

@API @Chrome @Cleanup
Scenario: Basic contact test
Given a contact named TestContact with the following values
    | Property   | Value |
    | First Name | John  |
    | Last Name  | Smith |
    | Job Title  | CLO   |
When TestContact is updated with the following values
    | Property   | Value                  |
    | First Name | Jerry                  |
    | Job Title  | Chief Lazyness Officer |
Then TestContact has the following values
    | Property   | Value                  |
    | First Name | Jerry                  |
    | Last Name  | Smith                  |
    | Job Title  | Chief Lazyness Officer |

@API @Chrome @Cleanup
Scenario: Clearing values of Account
Given an account named TestAccount with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Description  | Test multi line         |
When TestAccount is updated with the following values
	| Property    | Value |
	| Industry    |       |
	| Website     |       |
	| Description |       |
Then TestAccount has the following values
	| Property     | Value        |
	| Account Name | DynamicHands |
	| Main Phone   | 0612345678   |
	| Website      |              |
	| Industry     |              |
	| Description  |              |