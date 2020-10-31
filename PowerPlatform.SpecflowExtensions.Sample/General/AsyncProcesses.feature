Feature: AsyncProcesses

@Cleanup @API
Scenario: Wait for Async processes to finish
Given an account named TestAccount with the following values
    | Property     | Value     |
    | Account Name | AsyncTest |
And a contact named TestContact with the following values
    | Property     | Value       |
    | First Name   | John        |
    | Last Name    | Smith       |
    | Company Name | TestAccount |
When all asynchronous processes for TestContact are finished
Then TestAccount has the following values
    | Property        | Value |
    | Contact Present | Yes   |

@Cleanup @API
Scenario: Running On Demand Workflow - Sync
Given an account named TestAccount with the following values
    | Property     | Value          |
    | Account Name | OnDemandWFTest |
When the workflow 'Account - Update Account Number Sync' is executed on TestAccount
Then TestAccount has the following values
    | Property       | Value          |
    | Account Number | UpdatedSync    |

@Cleanup @API
Scenario: Running On Demand Workflow - Async
Given an account named TestAccount with the following values
    | Property     | Value          |
    | Account Name | OnDemandWFTest |
When the workflow 'Account - Update Account Number Async' is executed on TestAccount
Then TestAccount has the following values
    | Property       | Value           |
    | Account Number | UpdatedAsync    |