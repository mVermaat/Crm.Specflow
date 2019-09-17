Feature: LookupDataType

@Chrome @Cleanup
Scenario: Lookup with multiple results tests
Given an account named FirstAccount with the following values
    | Property     | Value                   |
    | Account Name | DynamicHands            |
    | Main Phone   | 0612345678              |
    | Website      | https://dynamichands.nl |
    | Industry     | Consulting              |
And an account named SecondAccount with the following values
    | Property     | Value                   |
    | Account Name | DynamicHands            |
    | Main Phone   | 0612345678              |
    | Website      | https://dynamichands.nl |
    | Industry     | Consulting              |
When a contact named TestLookup is created with the following values
    | Property     | Value            |
    | First Name   | Jerry            |
    | Last Name    | Smith            |
    | Company Name | SecondAccount    |
    | Email        | someone@test.com |
Then TestLookup has the following values
    | Property     | Value            |
    | First Name   | Jerry            |
    | Last Name    | Smith            |
    | Company Name | SecondAccount    |
    | Email        | someone@test.com |

@Chrome @Cleanup
Scenario: Lookup test - Special Characters
Given an account named FirstAccount with the following values
    | Property     | Value                                 |
    | Account Name | Thïs \s a ' Special " Chácactèr Test/ |
    | Main Phone   | 0612345678                            |
    | Website      | https://dynamichands.nl               |
    | Industry     | Consulting                            |
When a contact named TestLookup is created with the following values
    | Property     | Value            |
    | First Name   | Jerry            |
    | Last Name    | Smith            |
    | Company Name | FirstAccount    |
    | Email        | someone@test.com |
Then TestLookup has the following values
    | Property     | Value            |
    | First Name   | Jerry            |
    | Last Name    | Smith            |
    | Company Name | FirstAccount     |
    | Email        | someone@test.com |

@API @Cleanup
Scenario: Assigning alias to a lookup value
Given an account named TestAccount with the following values
	| Property     | Value                  |
	| Account Name | Multiple Notifications |
	| Credit Limit | 10000                  |
	| Industry     | Brokers                |
When all asynchronous processes for TestAccount are finished
Then TestAccount's Auto Generated is named AutoGenRecord
And AutoGenRecord has the following values
	| Property     | Value |
	| Credit Limit | 10000 |
