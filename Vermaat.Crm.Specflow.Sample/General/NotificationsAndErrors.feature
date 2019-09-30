Feature: NotificationsAndErrors


@Chrome @Cleanup
Scenario: Single form notification
Given an account named NotificationAccount with the following values
	| Property     | Value               |
	| Account Name | Single Notification |
	| Credit Limit | 10000               |
Then NotificationAccount has the following form notifications
	| Message                                             | Level   |
	| The credit limit of this account is under 1.000.000 | Warning |

@Chrome @Cleanup
Scenario: Multiple form notifications
Given an account named NotificationAccount with the following values
	| Property     | Value                  |
	| Account Name | Multiple Notifications |
	| Credit Limit | 10000                  |
	| Industry     | Brokers                |
Then NotificationAccount has the following form notifications
	| Message                                                                | Level   |
	| The credit limit of this account is under 1.000.000                    | Warning |
	| This account is from the broker industry. Additional rules applicable. | Warning |

@Chrome @Cleanup @ExpectedError
Scenario: Required field not filled error notification
When an account named NotificationAccount is created with the following values
	| Property     | Value |
	| Account Name |       |
	| Credit Limit | 10000 |
Then the following form notifications are on the current form
	| Message                                             | Level   |
	| Account Name : Required fields must be filled in.   | Error   |
	| The credit limit of this account is under 1.000.000 | Warning |

@Cleanup @ExpectedError @Chrome
Scenario: Verify error popup
When an account named TestAccount is created with the following values
		 | Property     | Value             |
		 | Account Name | Test              |
		 | Website      | https://error.com |
Then the following error message appears: 'Website refers to error.com'