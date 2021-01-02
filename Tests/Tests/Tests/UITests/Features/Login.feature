Feature: Login
	In order to use full features of the Petbook app
	As a registered user
	I want to be able to sign in

@mytag
Scenario: Log in with correct credentials
	Given I am registered user
	When I login to application
	Then the photos page and personal links are visible

@mytag
Scenario: Try to log in with not-existing username
	Given the username does not exist
	When I try login to application
	Then the user is not redirected and toast message is visible

@mytag
Scenario: Try to log in with incorrect password
	Given the user uses incorrect password
	When I try login to application
	Then the user is not redirected and toast message is visible