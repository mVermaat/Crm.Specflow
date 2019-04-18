Feature: ContactTests
Some tests specific to the contact entity. 
Used to show tests that use unusual fields like the address and composite fields

@API @Cleanup
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

@API @Cleanup
Scenario: Lookup with multiple results tests
Given an account named FirstAccount with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
And an account named SecondAccount with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
When a contact named TestLookup is created with the following values
	| Property     | Value            |
	| First Name   | Jerry            |
	| Last Name    | Smith            |
	| Company Name | SecondAccount    |
	| Email        | someone@test.com |
Then TestLookup has the following values
	| Property     | Value            |
	| First Name   | Jerry            |
	| Last Name    | Smith            |
	| Company Name | SecondAccount    |
	| Email        | someone@test.com |