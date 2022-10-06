Feature: DeleteData

@API @Chrome @Firefox @Cleanup @Set1
Scenario: Delete a contact
	Given an account named TestAccount with the following values
		| Property     | Value                   |
		| Account Name | DynamicHands            |
		| Main Phone   | 0612345678              |
		| Website      | https://dynamichands.nl |
		| Industry     | Consulting              |
		| Description  | Test Deletes            |
	And a related contact from TestAccount named ChildContact with the following values
		| Property   | Value  |
		| First Name | Child  |
		| Last Name  | Record |
	When ChildContact is deleted
	Then TestAccount has the following values
		| Property        | Value |
		| Contact Deleted | Yes   |
	And within 30 seconds no contact exists with the following values
		| Property     | Value       |
		| Company Name | TestAccount |