Feature: MultiSelectOptionSet

@API @Chrome @Firefox @Cleanup @Set1
Scenario: Create Account - Check two option fields
	When an account named TestAccount is created with the following values
		| Property      | Value                     |
		| Account Name  | MultiOptionSet            |
		| Subscriptions | Donald Duck, Katrien Duck |
	Then TestAccount has the following values
		| Property      | Value                     |
		| Account Name  | MultiOptionSet            |
		| Subscriptions | Donald Duck, Katrien Duck |