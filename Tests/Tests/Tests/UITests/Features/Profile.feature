Feature: Profile
	In order to use Petbook app with personal account
	As a user
	I would like to see my profile and possibility to edit my data

Scenario: User can see its own profile details using menu
	Given I am existing user
	And I am logged in
	When I go to My Profile
	Then I see my account data


Scenario: User can edit its account via button in My Profile page
	Given I am new user
	And I am at my current account
	When I go to edition of My Profile
	And I edit pet fields Username = "", Age = 4, City = "Zabrze", Gender = "female", Description = "New pet description - Quis labore nostrud minim dolor."
	And I save current changes
	Then I see edited data of my account


Scenario: User can edit its account via button in right navigation menu 
	Given I am existing user
	And I am logged in
	When I go to edition of My Profile via button in right navigation menu
	And I edit pet fields Username = "", Age = 2, City = "Radom", Gender = "male", Description = "Edition via account btn"
	And I save current changes
	Then I see edited data of my account


Scenario: User can cancel profile edition without saving changes
	Given I am existing user
	And I am at my current account
	When I go to edition of My Profile
	And I edit pet fields Username = "NewName", Age = 1, City = "Katowice", Gender = "male", Description = "Test"
	And I cancel current changes
	Then I see old data of my account