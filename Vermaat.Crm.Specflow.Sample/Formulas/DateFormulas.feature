Feature: DateFormulas


@API @Cleanup
Scenario: Test Date Formulas

When an appointment named TestAppointment is created with the following values
	| Property   | Value                 |
	| Subject    | ="Formula " + Today() |
	| Start Time | 5-5-2019 14:00        |
	| End Time   | =Today()              |
Then TestAppointment has the following values
	| Property   | Value                 |
	| Subject    | ="Formula " + Today() |
	| Start Time | 5-5-2019 14:00        |
	| End Time   | =Today()              |