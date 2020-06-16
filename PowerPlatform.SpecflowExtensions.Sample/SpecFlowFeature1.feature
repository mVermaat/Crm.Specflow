Feature: SpecFlowFeature1

@API @Cleanup
Scenario: When a record is created, it should be added to the cache
	Then The alias cache has the following records
	| Value |
	|       |