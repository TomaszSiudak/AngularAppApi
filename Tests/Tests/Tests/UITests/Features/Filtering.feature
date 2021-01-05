Feature: Filtering
	In order to use search other pets according to desired criteria
	As a user
	I would like to be able to filter results

@mytag
Scenario Outline: Filters pets by Gender
	Given I am logged in at photos url
	When I filter pets by Gender "<gender>"
	Then I see only pets with given criteria
Examples:
    | gender |
    | Male   |
	| Female |