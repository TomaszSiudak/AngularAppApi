Feature: Like
	In order to show other users that I like their profile
	As a user
	I would like to have possibility to add like for them


Scenario: User can add like to other user at Pets list and it is visible at liked profile
	Given I am logged in as default user
	When I like the profile of random user at Pets list
	Then the liked user see it on its own profile

Scenario: User can add like at visited profile and it is visible at liked profile
	Given I am logged in as default user
	When I like the visited profile
	Then the liked user see it on its own profile