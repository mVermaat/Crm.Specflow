Feature: LeadTests

@API @Chrome @Cleanup
Scenario: Lead Qualification
Given a lead named ToQualify with the following values
	| Property     | Value              |
	| First Name   | Qualify            |
	| Last Name    | Test               |
	| Topic        | Qualification Test |
	| Company Name | Qualify Account    |
When ToQualify is qualified to a
	| Account | Opportunity | Contact |
	| true    | true        | true    |
Then an account named QualifyAccount exists with the following values
	| Property         | Value           |
	| Originating Lead | ToQualify       |
	| Account Name     | Qualify Account |
And a contact exists with the following values
	| Property         | Value          |
	| Originating Lead | ToQualify      |
	| Company Name     | QualifyAccount |
	| First Name       | Qualify        |
	| Last Name        | Test           |
And an opportunity exists with the following values
	| Property         | Value              |
	| Originating Lead | ToQualify          |
	| Topic            | Qualification Test |