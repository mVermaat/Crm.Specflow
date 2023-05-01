Feature: EmailTemplateTests

This feature creates an email template. The email template entity has a unusual attribute type: 'EntityName'
This scenario will verify the code regarding that attribute type works

@API @Cleanup @Set2
Scenario: Test the creation of an email template
	Given a template named MyTemplate with the following values
		| Property      | Value      |
		| title         | Test       |
		| Subject       | EML_601    |
		| Template Type | systemuser |