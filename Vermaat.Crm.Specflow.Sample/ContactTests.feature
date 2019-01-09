Feature: ContactTests
Some tests specific to the contact entity. 
Used to show tests that use unusual fields like the address and composite fields

@API @Cleanup
Scenario: Basic contact test
Given an existing contact named TestContact with the following values
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

