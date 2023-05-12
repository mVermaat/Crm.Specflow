Feature: ActivityPartyDataType

@API @Chrome @Firefox @Cleanup @Set2
Scenario: Create Phonecall record
	Given an account named TestAccount with the following values
		| Property     | Value         |
		| Account Name | ActivityParty |
	And a related contact from TestAccount named ChildContact with the following values
		| Property   | Value  |
		| First Name | Child  |
		| Last Name  | Record |
	And the current logged in user named MyUser
	When a phonecall named MyPhoneCall is created with the following values
		| Property  | Value                                             |
		| Call From | MyUser                                            |
		| Call To   | TestAccount, ChildContact, doesntexist@domain.com |
		| Subject   | SpecFlow phonecall                                |
	Then MyPhoneCall has the following values
		| Property  | Value                                             |
		| Call From | MyUser                                            |
		| Call To   | TestAccount, ChildContact, doesntexist@domain.com |
		| Subject   | SpecFlow phonecall                                |