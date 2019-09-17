Feature: Merging

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