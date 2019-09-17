Feature: StringDataType

@API @Chrome @Cleanup
Scenario: Test text fields
When an account named TestAccount is created with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 06123456789             |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Description  | Test multi line         |
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 06123456789             |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Description  | Test multi line         |
