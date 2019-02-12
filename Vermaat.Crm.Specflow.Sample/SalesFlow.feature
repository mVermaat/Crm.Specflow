Feature: Salesflow
	Tests for the lead/opportunity entity to show special dialogs like the qualify lead, process flows and related entities

Scenario: Lead Qualification
Given a lead named ToQualify with the following values
		| Property | Value              |
		| Topic    | Qualification Test |
		| Name     | Qualify            |
When ToQualify is qualified to a
	| Account | Opportunity | Contact |
	| true    | true        | true    |