Feature: StringDataType

@API @Chrome @Firefox @Cleanup @Set1
Scenario: Test text fields
	When an account named TestAccount is created with the following values
		| Property      | Value                   |
		| Account Name  | DynamicHands            |
		| Main Phone    | 06123456789             |
		| Website       | https://dynamichands.nl |
		| Industry      | Consulting              |
		| Description   | Test multi line         |
		| Ticker Symbol | ABC                     |
	Then TestAccount has the following values
		| Property       | Condition | Value                   |
		| Account Name   | Equal     | DynamicHands            |
		| Ticker Symbol  | NotNull   |                         |
		| Main Phone     |           | 06123456789             |
		| Website        | Equal     | https://dynamichands.nl |
		| Industry       | Equal     | Consulting              |
		| Description    |           | Test multi line         |
		| Account Number | Regex     | ^AC-\\d{8}$             |

# ^AC-\d{8}$  <-- AC- then 8 numbers exactly. No text before or after