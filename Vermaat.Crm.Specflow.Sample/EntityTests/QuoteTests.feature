Feature: QuoteTests

@API @Chrome @Cleanup
Scenario: Convert Quote to Sales Order
Given an account named QuoteTesting with the following values
	| Property              | Value                   |
	| Account Name          | QuoteTesting            |
And a quote named TestQuote with the following values
	| Property           | Value                |
	| Name               | AT Quote             |
	| Price List         | Automated Testing PL |
	| Potential Customer | QuoteTesting         |

When TestQuote is activated and converted to a sales order named TestOrder
Then TestOrder has the following values
	| Property   | Value                |
	| Name       | AT Quote             |
	| Price List | Automated Testing PL |
	| Customer   | QuoteTesting         |
	| Quote      | TestQuote            |

@API @Chrome @Cleanup
Scenario: Revise a quote
Given an account named QuoteTesting with the following values
	| Property              | Value                   |
	| Account Name          | QuoteTesting            |
And a quote named TestQuote with the following values
	| Property           | Value                |
	| Name               | AT Quote to Revise   |
	| Price List         | Automated Testing PL |
	| Potential Customer | QuoteTesting         |
When the quote TestQuote is activated
And TestQuote is revised and its revised quote is named RevisiedQuote
Then RevisiedQuote has the following values
	| Property           | Value                |
	| Name               | AT Quote to Revise   |
	| Price List         | Automated Testing PL |
	| Potential Customer | QuoteTesting         |
