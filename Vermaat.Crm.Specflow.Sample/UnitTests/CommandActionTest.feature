Feature: CommandActionTest

@API @Chrome @Firefox
Scenario: No Command Action Tag
Then The command action is set to Default


@API @Chrome @Firefox @ForceAPI
Scenario: ForceAPI Command Action Tag
Then The command action is set to ForceApi

@API @Chrome @Firefox @ForceBrowser
Scenario: ForceBrowser Command Action Tag
Then The command action is set to ForceBrowser

@API @Chrome @Firefox @PreferAPI
Scenario: PreferApi Command Action Tag
Then The command action is set to PreferApi

@API @Chrome @Firefox @PreferBrowser
Scenario: PreferBrowser Command Action Tag
Then The command action is set to PreferBrowser