Feature: AccountTests
	Some tests involving the account entity

@API @Chrome @Cleanup
Scenario: Create a new Account
When an account named TestAccount is created with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 06123456789             |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 06123456789             |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |

@API @Chrome @Cleanup
Scenario: Update an existing Account
Given an account named TestAccount with the following values
	| Property              | Value                   |
	| Account Name          | DynamicHands            |
	| Main Phone            | 0612345678              |
	| Website               | https://dynamichands.nl |
	| Industry              | Consulting              |
When TestAccount is updated with the following values
	| Property     | Value             |
	| Account Name | DynamicHands B.V. |
	| Main Phone   | 06987654321       |
	| Fax          | 4839432324        |	
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands B.V.       |
	| Main Phone   | 06987654321             |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |


@Chrome @Cleanup
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

@API @Chrome @Cleanup
Scenario: Create Account - Check two option fields
When an account named TestAccount is created with the following values
	| Property                 | Value        |
	| Account Name             | Checkbox     |
	| Do not allow Phone Calls | Do Not Allow |
	| Do not allow Faxes       | Do Not Allow |
	| Do not allow Mails       | Do Not Allow |
Then TestAccount has the following values
	| Property                 | Value        |
	| Account Name             | Checkbox     |
	| Do not allow Phone Calls | Do Not Allow |
	| Do not allow Faxes       | Do Not Allow |
	| Do not allow Mails       | Do Not Allow |

@API @Chrome @Cleanup
Scenario: Clearing values of Account
Given an account named TestAccount with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
When TestAccount is updated with the following values
	| Property | Value |
	| Industry |       |
	| Website  |       |
Then TestAccount has the following values
	| Property     | Value        |
	| Account Name | DynamicHands |
	| Main Phone   | 0612345678   |
	| Website      |              |
	| Industry     |              |

@Chrome @Cleanup
Scenario: Setting values while switching tabs
When an account named TestAccount is created with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Industry     | Consulting              |
	| Main Phone   | 0612345678              |
	| Credit Hold  | Yes                     |
	| Website      | https://dynamichands.nl |
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Credit Hold  | Yes                     |

@API @Cleanup
Scenario: Full Merge of two accounts into one
Given an account named MergeSource with the following values
	| Property     | Value          |
	| Account Name | Merging Source |
	| Main Phone   | 0612345678     |
Given an account named MergeTarget with the following values
	| Property     | Value                   |
	| Account Name | Merging Target          |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
When MergeSource is fully merged into MergeTarget
Then MergeTarget has the following values
	| Property     | Value                   |
	| Account Name | Merging Source          |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Main Phone   | 0612345678              |

@API @Cleanup
Scenario: Partial Merge of two accounts into one
Given an account named MergeSource with the following values
	| Property     | Value          |
	| Account Name | Merging Source |
	| Main Phone   | 0612345678     |
Given an account named MergeTarget with the following values
	| Property     | Value                   |
	| Account Name | Merging Target          |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
When The following fields of MergeSource are fully merged into MergeTarget
	| Property     |
	| Main Phone   |
Then MergeTarget has the following values
	| Property     | Value                   |
	| Account Name | Merging Target          |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
	| Main Phone   | 0612345678              |

@API @Chrome @Cleanup
Scenario: Use DefaultData for default values
When an account named TestAccount is created with the following values
	| Property     | Value |                 
Then TestAccount has the following values
	| Property     | Value |
	| Account Name | test  |


@API @Chrome @Cleanup
Scenario: Creating child contact from account
Given an account named TestAccount with the following values
	| Property                   | Value                   |
	| Account Name               | ParentAccount           |
	| Main Phone                 | 0612345678              |
	| Website                    | https://dynamichands.nl |
	| Address 1: Street 1        | Street 1                |
	| Address 1: Street 2        | Street 2                |
	| Address 1: Street 3        | Street 3                |
	| Address 1: City            | City                    |
	| Address 1: State/Province  | State                   |
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