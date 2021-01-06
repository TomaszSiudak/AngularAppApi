Feature: Filtering
	In order to use search other pets according to desired criteria
	As a user
	I would like to be able to filter results

@mytag
Scenario Outline: Filters pets by Gender
	Given I am logged in at photos url
	When I filter pets by Gender "<gender>"
	Then I see only pets with given gender
Examples:
    | gender |
    | Male   |
	| Female |


Scenario Outline: Filters pets by Type
	Given I am logged in at photos url
	When I filter pets by Type "<type>"
	Then I see only pets with given type
Examples:
    | type   |
    | Dog    |
	| Cat	 |
	| Rabbit |
	| Parrot |
	| Hamster|
