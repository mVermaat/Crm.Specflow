Feature: SpecFlowFeature1

@API @Chrome @Cleanup
Scenario: When a record is created, it should be added to the cache
    When an contact named TestContact is created with the following values
    | Property   | Value |
    | First Name | John  |
    | Last Name  | Smith |
    | Job Title  | CLO   |
    Then The alias cache has the following records
    | Value       |
    | TestContact |