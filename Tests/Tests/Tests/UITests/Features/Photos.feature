Feature: Photos
	In order to present my profile attractively
	As a user
	I would like to manage my photos

@mytag
Scenario: User can add new photo to profile
	Given I am existing user
	And I am at my current account
	When I go to edition of My Profile
	And I upload new photo
	And I save
	Then I see new photo added to my account

Scenario: User can cancel upload of new photo
	Given I am existing user
	And I am at my current account
	When I go to edition of My Profile
	And I upload new photo
	And I cancel upload
	Then I do not see new photo added to my account