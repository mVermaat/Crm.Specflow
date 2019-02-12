Feature: Salesflow
	Tests for the lead/opportunity entity to show special dialogs like the qualify lead, process flows and related entities

@API @Chrome
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
Then an account exists with the following values
	| Property         | Value           |
	| Originating Lead | ToQualify       |
	| Account Name     | Qualify Account |
And a contact exists with the following values
	| Property         | Value     |
	| Originating Lead | ToQualify |
	| First Name       | Qualify   |
	| Last Name        | Test      |
And an opportunity exists with the following values
	| Property         | Value              |
	| Originating Lead | ToQualify          |
	| Topic            | Qualification Test |