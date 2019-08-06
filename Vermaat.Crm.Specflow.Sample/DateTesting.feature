Feature: DateTesting


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
	
