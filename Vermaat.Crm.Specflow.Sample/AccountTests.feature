Feature: AccountTests
	Some tests involving the account entity

@API @Chrome @Cleanup
Scenario: Create a new Account
When an account named TestAccount is created with the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
	| Website      | https://dynamichands.nl |
	| Industry     | Consulting              |
Then TestAccount has the following values
	| Property     | Value                   |
	| Account Name | DynamicHands            |
	| Main Phone   | 0612345678              |
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
Then TestAccount's form has the following visbility
	| Property  | Visible |
	| SIC Code  | True    |
	| Ownership | False   |

@Chrome @Cleanup
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