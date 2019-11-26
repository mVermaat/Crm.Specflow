Feature: DateTimeDataType

# Make sure your user is a non UTC timezone
@Cleanup @API @Chrome
Scenario: Test all Date and Time formats
Given a mv_datetester named TestDateTime with the following values
	| Property                        | Value          |
	| Name                            | Testing Dates  |
	| Date Only                       | 1-1-2010       |
	| User Local Date Only            | 1-1-2010       |
	| User Local Date Time            | 1-1-2010 10:00 |
	| Time Zone Independent Date Only | 1-1-2010       |
	| Time Zone Independent Date Time | 1-1-2010 10:00 |
Then TestDateTime has the following values
	| Property                        | Value          |
	| Date Only                       | 1-1-2010       |
	| User Local Date Only            | 1-1-2010       |
	| User Local Date Time            | 1-1-2010 10:00 |
	| Time Zone Independent Date Only | 1-1-2010       |
	| Time Zone Independent Date Time | 1-1-2010 10:00 |
	
@Cleanup @API @Chrome
Scenario: Test updating all Date and Time formats
Given a mv_datetester named TestDateTime with the following values
	| Property                        | Value          |
	| Name                            | Testing Dates  |
	| Date Only                       | 1-1-2010       |
	| User Local Date Only            | 1-1-2010       |
	| User Local Date Time            | 1-1-2010 10:00 |
	| Time Zone Independent Date Only | 1-1-2010       |
	| Time Zone Independent Date Time | 1-1-2010 10:00 |
When TestDateTime is updated with the following values
	| Property                        | Value          |
	| Date Only                       | 2-2-2011       |
	| User Local Date Only            | 2-2-2011       |
	| User Local Date Time            | 2-2-2011 12:00 |
	| Time Zone Independent Date Only | 2-2-2011       |
	| Time Zone Independent Date Time | 2-2-2011 12:00 |
Then TestDateTime has the following values
	| Property                        | Value          |
	| Date Only                       | 2-2-2011       |
	| User Local Date Only            | 2-2-2011       |
	| User Local Date Time            | 2-2-2011 12:00 |
	| Time Zone Independent Date Only | 2-2-2011       |
	| Time Zone Independent Date Time | 2-2-2011 12:00 |

@Cleanup @API @Chrome
Scenario: Test clearing all Date and Time formats
Given a mv_datetester named TestDateTime with the following values
	| Property                        | Value          |
	| Name                            | Testing Dates  |
	| Date Only                       | 1-1-2010       |
	| User Local Date Only            | 1-1-2010       |
	| User Local Date Time            | 1-1-2010 10:00 |
	| Time Zone Independent Date Only | 1-1-2010       |
	| Time Zone Independent Date Time | 1-1-2010 10:00 |
When TestDateTime is updated with the following values
	| Property                        | Value |
	| Date Only                       |       |
	| User Local Date Only            |       |
	| User Local Date Time            |       |
	| Time Zone Independent Date Only |       |
	| Time Zone Independent Date Time |       |
Then TestDateTime has the following values
	| Property                        | Value |
	| Date Only                       |       |
	| User Local Date Only            |       |
	| User Local Date Time            |       |
	| Time Zone Independent Date Only |       |
	| Time Zone Independent Date Time |       |