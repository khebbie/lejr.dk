@Specs
Feature: ActiveCamps
	In order to find camps that I am interested in
	As a user
	I want to see the upcomming camps

@Specs
Scenario: No upcomming camps exists
	Given No junior camps exists in the db
	When I go the junior camp page
	Then I get a message saying "Ingen planlagte lejre"

@Specs
Scenario: One upcomming camp
	Given 1 junior camps exists in the db
	When I go the junior camp page
	Then I see the camp information

@Specs
Scenario: Three upcomming camps
	Given 3 junior camps exists in the db
	When I go the junior camp page
	Then I see the camp information 