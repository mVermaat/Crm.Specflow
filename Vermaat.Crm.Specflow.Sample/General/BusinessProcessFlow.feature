Feature: BusinessProcessFlows


@API @Chrome @Firefox @Cleanup @Set2
Scenario: Moving across business process flow steps
	Given an account named TestAccount with the following values
		| Property     | Value       |
		| Account Name | TestAccount |
	And a related opportunity from TestAccount named TestOpp with the following values
		| Property | Value        |
		| Topic    | Test Opp BPF |
	And TestOpp has the process stage Propose
	When TestOpp is moved to the next process stage
	Then the process stage of TestOpp is Close


@Chrome @Firefox @Cleanup @Set2
Scenario: Setting BPF Fields
	Given an account named TestAccount with the following values
		| Property     | Value       |
		| Account Name | TestAccount |
	When a related opportunity from TestAccount named TestOpp is created with the following values
		| Property            | Value            |
		| Topic               | Test Opp BPF     |
		| BPF Boolean Field   | Yes              |
		| BPF Currency Field  | 1000.78          |
		| BPF DateTime Field  | 10-10-2023 15:00 |
		| BPF Decimal Field   | 1234.86          |
		| BPF Float Field     | 43.67            |
		| BPF Integer Field   | 855              |
		| BPF Lookup Field    | TestAccount      |
		| BPF OptionSet Field | Donald Duck      |
		| BPF Text Field      | Oh Snap          |
	Then TestOpp has the following values
		| Property            | Value            |
		| Topic               | Test Opp BPF     |
		| BPF Boolean Field   | Yes              |
		| BPF Currency Field  | 1000.78          |
		| BPF DateTime Field  | 10-10-2023 15:00 |
		| BPF Decimal Field   | 1234.86          |
		| BPF Float Field     | 43.67            |
		| BPF Integer Field   | 855              |
		| BPF Lookup Field    | TestAccount      |
		| BPF OptionSet Field | Donald Duck      |
		| BPF Text Field      | Oh Snap          |

@Chrome @Firefox @Cleanup @Set2
Scenario: Clearing BPF Fields
	Given an account named TestAccount with the following values
		| Property     | Value       |
		| Account Name | TestAccount |
	And a related opportunity from TestAccount named TestOpp with the following values
		| Property            | Value            |
		| Topic               | Test Opp BPF     |
		| BPF Boolean Field   | Yes              |
		| BPF Currency Field  | 1000.78          |
		| BPF DateTime Field  | 10-10-2023 15:00 |
		| BPF Decimal Field   | 1234.86          |
		| BPF Float Field     | 43.67            |
		| BPF Integer Field   | 855              |
		| BPF Lookup Field    | TestAccount      |
		| BPF OptionSet Field | Donald Duck      |
		| BPF Text Field      | Oh Snap          |
	When TestOpp is updated with the following values
		| Property            | Value |
		| BPF Boolean Field   |       |
		| BPF Currency Field  |       |
		| BPF DateTime Field  |       |
		| BPF Decimal Field   |       |
		| BPF Float Field     |       |
		| BPF Integer Field   |       |
		| BPF Lookup Field    |       |
		| BPF OptionSet Field |       |
		| BPF Text Field      |       |
	Then TestOpp has the following values
		| Property            | Value            |
		| Topic               | Test Opp BPF     |
		| BPF Boolean Field   | Yes              |
		| BPF Currency Field  | 1000.78          |
		| BPF DateTime Field  | 10-10-2023 15:00 |
		| BPF Decimal Field   | 1234.86          |
		| BPF Float Field     | 43.67            |
		| BPF Integer Field   | 855              |
		| BPF Lookup Field    | TestAccount      |
		| BPF OptionSet Field | Donald Duck      |
		| BPF Text Field      | Oh Snap          |



