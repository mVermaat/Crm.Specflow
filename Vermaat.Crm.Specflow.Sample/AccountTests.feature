# Check if field is visible on the form
# What if tab is collapsed?

Feature: AccountTests
	Some tests involving the account entity

@API @Chrome @Cleanup
Scenario: Create a new Account
When an account named TestAccount is created with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |

@API @Chrome @Cleanup
Scenario: Update an existing Account
Given an account named TestAccount with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
When TestAccount is updated with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands B.V.       |
	| Main Phone   | 06987654321             |
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands B.V.       |
	| Main Phone   | 06987654321             |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |