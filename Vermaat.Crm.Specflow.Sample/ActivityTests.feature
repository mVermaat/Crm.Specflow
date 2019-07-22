Feature: ActivityTests
	

@API @Chrome @Cleanup
Scenario: Filling DateTime fields for appointment
Given an appointment named TestAppointment with the following values
	| Property   | Value           |
	| Subject    | TestDateTime    |
	| Start Time | 20-5-2018 10:00 |
	| End Time   | 20-5-2018 12:00 |
Then TestAppointment has the following values
	| Property   | Value           |
	| Subject    | TestDateTime    |
	| Start Time | 20-5-2018 10:00 |
	| End Time   | 20-5-2018 12:00 |