Feature: SwitchTabs

@Chrome @Cleanup
Scenario: Setting values while switching tabs
When an account named TestAccount is created with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Industry     | Consulting              |
	| Main Phone   | 0612345678              |
	| Credit Hold  | Yes                     |
	| Website      | https://dynamichands.nl |
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Credit Hold  | Yes                     |