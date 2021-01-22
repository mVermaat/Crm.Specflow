Feature: BusinessProcessFlows


@API @Cleanup
Scenario: Moving across business process flow steps
Given an account named TestAccount with the following values
	| Property     | Value       |
	| Account Name | TestAccount |
And a related opportunity from TestAccount named TestOpp with the following values
	| Property | Value        |
	| Topic    | Test Opp BPF |
And TestOpp has the process stage Propose
When TestOpp is moved to the next process stage
Then the process stage of TestOpp is Close