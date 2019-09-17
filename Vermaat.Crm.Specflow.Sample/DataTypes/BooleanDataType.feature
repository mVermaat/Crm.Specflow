Feature: BooleanDataType

@API @Chrome @Cleanup
Scenario: Create Account - Check two option fields
When an account named TestAccount is created with the following values
	| Property                 | Value        |
	| Account Name             | Checkbox     |
	| Do not allow Phone Calls | Do Not Allow |
	| Do not allow Faxes       | Do Not Allow |
	| Do not allow Mails       | Do Not Allow |
Then TestAccount has the following values
	| Property                 | Value        |
	| Account Name             | Checkbox     |
	| Do not allow Phone Calls | Do Not Allow |
	| Do not allow Faxes       | Do Not Allow |
	| Do not allow Mails       | Do Not Allow |