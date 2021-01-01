Feature: Login
	In order to use full features of the Petbook app
	As a registered user
	I want to be able to sign in

@mytag
Scenario: Log in with correct credentials
	Given I am registered user
	When I enter correct login and password
	And I click Zaloguj się button
	Then the photos page and personal links are visible