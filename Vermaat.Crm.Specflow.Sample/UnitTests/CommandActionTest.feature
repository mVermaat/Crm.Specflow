Feature: CommandActionTest

@API @Chrome @Firefox @Set1
Scenario: No Command Action Tag
Then The command action is set to Default


@API @Chrome @Firefox @ForceAPI @Set1
Scenario: ForceAPI Command Action Tag
Then The command action is set to ForceApi

@API @Chrome @Firefox @ForceBrowser @Set1
Scenario: ForceBrowser Command Action Tag
Then The command action is set to ForceBrowser

@API @Chrome @Firefox @PreferAPI @Set1
Scenario: PreferApi Command Action Tag
Then The command action is set to PreferApi

@API @Chrome @Firefox @PreferBrowser @Set1
Scenario: PreferBrowser Command Action Tag
Then The command action is set to PreferBrowser