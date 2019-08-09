Feature: ContactTests
Some tests specific to the contact entity. 
Used to show tests that use unusual fields like the address and composite fields

@API @Chrome @Cleanup
Scenario: Basic contact test
Given a contact named TestContact with the following values
    | Property   | Value |
    | First Name | John  |
    | Last Name  | Smith |
    | Job Title  | CLO   |
When TestContact is updated with the following values
    | Property   | Value                  |
    | First Name | Jerry                  |
    | Job Title  | Chief Lazyness Officer |
Then TestContact has the following values
    | Property   | Value                  |
    | First Name | Jerry                  |
    | Last Name  | Smith                  |
    | Job Title  | Chief Lazyness Officer |

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
Scenario: Check required of form items
Given a contact named TestContact with the following values
    | Property   | Value |
    | First Name | John  |
    | Last Name  | Smith |
    | Job Title  | CLO   |
Then TestContact's form has the following form state
    | Property           | State       |
    | First Name         | Recommended |
    | Last Name          | Required    |
    | Job Title          | Optional    |

@Chrome @Cleanup
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

@Chrome @Cleanup
Scenario: Check combined state of form items
Given a contact named TestContact with the following values
    | Property   | Value |
    | First Name | John  |
    | Last Name  | Smith |
    | Job Title  | CLO   |
Then TestContact's form has the following form state
    | Property                       | State                          |
    | First Name                     | Recommended, Unlocked, Visible |
    | Last Name                      | Required, Unlocked, Visible    |
    | Job Title                      | Optional, Unlocked, Visible    |
    | Last Date Included in Campaign | Locked, Optional, Visible      |

@Cleanup @Chrome @API
Scenario: Filling Date Only fields
Given a contact named TestContact with the following values
    | Property    | Value      |
    | First Name  | John       |
    | Last Name   | Smith      |
    | Job Title   | CLO        |
    | Birthday    | 20-05-1985 |
    | Anniversary | 10-10-2010 |
Then TestContact has the following values
    | Property    | Value      |
    | First Name  | John       |
    | Last Name   | Smith      |
    | Job Title   | CLO        |
    | Birthday    | 20-05-1985 |
    | Anniversary | 10-10-2010 |

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

@Chrome @Cleanup
Scenario: Basic Lookup test - Special Characters
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