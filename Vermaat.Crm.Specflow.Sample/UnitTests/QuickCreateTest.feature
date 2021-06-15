Feature: QuickCreateTest

@Chrome @Firefox @Cleanup
Scenario: Create a contact via quick create
	Given an account named TestAccount with the following values
		| Property     | Value        |
		| Account Name | Vermaat B.V. |
	When TestAccount has a contact named TestContact created via quick create with the following values
		| Property   | Value  |
		| First Name | Child  |
		| Last Name  | Record |
	Then TestContact has the following values
		| Property   | Value  |
		| First Name | Child  |
		| Last Name  | Record |
#	| Business Phone             | 0612345678    |
#	| Address 1: Street 1        | SomethingElse |
#	| Address 1: Street 2        |               |
#	| Address 1: Street 3        | Street 3      |
#	| Address 1: City            | City          |
#	| Address 1: State/Province  | State         |