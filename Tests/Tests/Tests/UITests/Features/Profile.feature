Feature: Profile
	In order to use Petbook app with personal account
	As a user
	I would like to see my profile and possibility to edit my data

@mytag
Scenario: Profile is filled correctly with Pet data
	Given I am logged in at photos url
	When I go to My Profile
	Then I see my account data

@mytag
Scenario: User can edit its account
	Given I am new user
	And I am at my current account
	When I go to edition of My Profile
	And I edit pet fields Username = "", Age = 4, City = "Zabrze", Gender = "female", Description = "New pet description - Quis labore nostrud minim dolor."
	And I save current changes
	Then I see my account data