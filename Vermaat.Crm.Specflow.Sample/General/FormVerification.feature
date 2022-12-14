Feature: FormVerification

@API @Set2 @Cleanup
Scenario Outline: Verify dashboard is accessible by roles - outline
	Then dashboard '<Name>' is accessible by following roles: <Roles>

Examples:
	| Name             | Roles                                                |
	| MV - Dashboard 1 | MV - Role 1;  MV - Role 2                            |
	| MV - Dashboard 2 | MV - Role 1; System Administrator; System Customizer |

@API @Set2 @Cleanup
Scenario: Verify dashboard is accessible by roles - table
	Then dashboard 'MV - Dashboard 1' is accessible by following roles
		| Role        |
		| MV - Role 1 |
		| MV - Role 2 |

@API @Set2 @Cleanup
Scenario Outline: Verify form is accessible by roles - outline
	Then form '<Name>' of <Entity> is accessible by following roles: <Roles>

Examples:
	| Name         | Entity  | Roles                                                              |
	| MV Role Form | account | MV - Role 1;  MV - Role 2; System Administrator; System Customizer |
	| MV Role Form | contact | MV - Role 1; System Administrator; System Customizer               |

@API @Set2 @Cleanup
Scenario: Verify form is accessible by roles - table
	Then form 'MV Role Form' of account is accessible by following roles
		| Role                 |
		| MV - Role 1          |
		| MV - Role 2          |
		| System Administrator |
		| System Customizer    |
