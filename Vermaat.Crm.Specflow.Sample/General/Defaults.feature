Feature: Defaults

@API @Chrome @Cleanup
Scenario: Use DefaultData for default values
When an account named TestAccount is created with the following values
	| Property     | Value |                 
Then TestAccount has the following values
	| Property     | Value |
	| Account Name | test  |