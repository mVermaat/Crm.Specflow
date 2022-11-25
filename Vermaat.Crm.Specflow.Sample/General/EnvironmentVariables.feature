Feature: EnvironmentVariables

@API @Set2 @Cleanup
Scenario Outline: Update Environment Variables
	Given the environment variable <Name> has the value <Value1>
	Then an environmentvariabledefinition named VarDef1 exists with the following values
		| Property   | Value  |
		| schemaname | <Name> |
	And an environmentvariablevalue named VarValue1 exists with the following values
		| Property                        | Value   |
		| environmentvariabledefinitionid | VarDef1 |
	And VarValue1 has the following values
		| Property | Value    |
		| value    | <Value1> |
	Given the environment variable <Name> has the value <Value2>
	Then an environmentvariabledefinition named VarDef2 exists with the following values
		| Property   | Value  |
		| schemaname | <Name> |
	And an environmentvariablevalue named VarValue2 exists with the following values
		| Property                        | Value   |
		| environmentvariabledefinitionid | VarDef2 |
	And VarValue2 has the following values
		| Property | Value    |
		| value    | <Value2> |

Examples:
	| Name          | Value1          | Value2               |
	| mv_BooleanVar | true            | false                |
	| mv_JsonVar    | {"some":"json"} | {"something":"else"} |
	| mv_NumberVar  | 1.6             | 3.6                  |
	| mv_TextVar    | SomeText        | SomethingElse        |