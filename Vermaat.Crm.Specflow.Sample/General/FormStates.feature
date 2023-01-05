Feature: FormStates

@Chrome @Firefox @Cleanup @Set2
Scenario: Check required of form items
	Given a contact named TestContact with the following values
		| Property   | Value |
		| First Name | John  |
		| Last Name  | Smith |
		| Job Title  | CLO   |
	Then TestContact's form has the following form state
		| Property   | State       |
		| First Name | Recommended |
		| Last Name  | Required    |
		| Job Title  | Optional    |

@Chrome @Firefox @Cleanup @Set2
Scenario: Check locked state of form items
	Given a contact named TestContact with the following values
		| Property   | Value |
		| First Name | John  |
		| Last Name  | Smith |
		| Job Title  | CLO   |
	Then TestContact's form has the following form state
		| Property                       | State    |
		| First Name                     | Unlocked |
		| Last Date Included in Campaign | Locked   |


@Chrome @Firefox @Cleanup @Set2
Scenario: Check visiblity of form items
	When an account named TestAccount is created with the following values
		| Property     | Value                   |
		| Account Name | DynamicHands            |
		| Main Phone   | 0612345678              |
		| Website      | https://dynamichands.nl |
		| Industry     | Consulting              |
	Then TestAccount's form has the following form state
		| Property  | State     |
		| SIC Code  | Visible   |
		| Ownership | Invisible |

@Chrome @Firefox @Cleanup @Set2
Scenario: Check combined state of form items
	Given a contact named TestContact with the following values
		| Property   | Value |
		| First Name | John  |
		| Last Name  | Smith |
		| Job Title  | CLO   |
	Then TestContact's form has the following form state
		| Property                       | Tab     | Section             | State                          |
		| First Name                     | Summary | CONTACT INFORMATION | Recommended, Unlocked, Visible |
		| Last Name                      | Summary | CONTACT INFORMATION | Required, Unlocked, Visible    |
		| Job Title                      | Summary | CONTACT INFORMATION | Optional, Unlocked, Visible    |
		| Last Date Included in Campaign | Details | MARKETING           | Locked, Optional, Visible      |

@Chrome @Firefox @Cleanup @Set2
Scenario: Ribbon button availability
	Given a account named TestAccount with the following values
		| Property     | Value        |
		| Account Name | DynamicHands |
	Then TestAccount's form has the following ribbon state
		| Property   | State     |
		| Activate   | Visible   |
		| Deactivate | Invisible |
		| Run Report | Visible   |


@Chrome @Firefox @Cleanup @Set2
Scenario: Check tab/section of fields
	Given a contact named TestContact with the following values
		| Property   | Value |
		| First Name | John  |
		| Last Name  | Smith |
		| Job Title  | CLO   |
	Then TestContact's form has the following form state
		| Property                       | Tab     | Section             |
		| First Name                     | Summary | CONTACT INFORMATION |
		| Last Name                      | Summary | CONTACT INFORMATIONa |
		| Job Title                      | Summary | CONTACT INFORMATION |
		| Last Date Included in Campaign | Detailss | MARKETING           |