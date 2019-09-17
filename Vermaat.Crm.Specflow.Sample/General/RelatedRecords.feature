Feature: RelatedRecords


@API @Chrome @Cleanup
Scenario: Creating child contact from account
Given an account named TestAccount with the following values
	| Property                   | Value                   |
	| Main Phone                 | 0612345678              |
	| Website                    | https://dynamichands.nl |
	| Address 1: Street 1        | Street 1                |
	| Address 1: Street 2        | Street 2                |
	| Address 1: Street 3        | Street 3                |
	| Address 1: City            | City                    |
	| Address 1: State/Province  | State                   |
	| Account Name               | ParentAccount           |
When a related contact from TestAccount named ChildContact is created with the following values
	| Property            | Value         |
	| First Name          | Child         |
	| Last Name           | Record        |
	| Address 1: Street 1 | SomethingElse |
	| Address 1: Street 2 |               |
Then ChildContact has the following values
	| Property                   | Value         |
	| First Name                 | Child         |
	| Last Name                  | Record        |
	| Business Phone             | 0612345678    |
	| Address 1: Street 1        | SomethingElse |
	| Address 1: Street 2        |               |
	| Address 1: Street 3        | Street 3      |
	| Address 1: City            | City          |
	| Address 1: State/Province  | State         |

@API @Cleanup
Scenario: Connect records via a N:N Relationship
Given an account named NNAccount with the following values
	| Property     | Value           |
	| Account Name | NN Relationship |
	| Main Phone   | 0612345678      |
And a contact named Contact1 with the following values
	| Property   | Value |
	| First Name | John  |
	| Last Name  | Smith |
And a contact named Contact2 with the following values
	| Property   | Value |
	| First Name | Bart  |
	| Last Name  | Pond  |
And a contact named Contact3 with the following values
	| Property   | Value   |
	| First Name | Eric    |
	| Last Name  | Foreman |
When the following records of type contact are connected to NNAccount
	| Value    |
	| Contact1 |
	| Contact2 |
	| Contact3 |
Then NNAccount has the following connected records of type contact
	| Value    |
	| Contact1 |
	| Contact2 |
	| Contact3 |