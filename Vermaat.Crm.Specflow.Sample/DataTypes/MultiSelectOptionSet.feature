Feature: MultiSelectOptionSet

@API @Chrome @Firefox @Cleanup @Set1
Scenario: Create Account - Check multi option field
	When an account named TestAccount is created with the following values
		| Property      | Value                     |
		| Account Name  | MultiOptionSet            |
		| Subscriptions | Donald Duck, Katrien Duck |
	Then TestAccount has the following values
		| Property      | Value                     |
		| Account Name  | MultiOptionSet            |
		| Subscriptions | Donald Duck, Katrien Duck |

@API @Chrome @Firefox @Cleanup @Set1
Scenario: Update Account - Clear multi option field
	Given an account named TestAccount with the following values
		| Property      | Value                     |
		| Account Name  | MultiOptionSet            |
		| Subscriptions | Donald Duck, Katrien Duck |
	When TestAccount is updated with the following values
		| Property      | Value |
		| Subscriptions |       |
	Then TestAccount has the following values
		| Property      | Value          |
		| Account Name  | MultiOptionSet |
		| Subscriptions |                |


@API @Chrome @Firefox @Cleanup @Set1
Scenario: Update Account - Check multi option field
	Given an account named TestAccount with the following values
		| Property      | Value                     |
		| Account Name  | MultiOptionSet            |
		| Subscriptions | Donald Duck, Katrien Duck |
	When TestAccount is updated with the following values
		| Property      | Value       |
		| Subscriptions | Donald Duck |
	Then TestAccount has the following values
		| Property      | Value          |
		| Account Name  | MultiOptionSet |
		| Subscriptions | Donald Duck    |