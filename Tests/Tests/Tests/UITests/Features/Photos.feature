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


Scenario: User can delete photo
	Given I am logged in as default user
	When I go to edition of My Profile via button in right navigation menu
	And I delete unwanted photo
	Then I see one photo less in my account


Scenario: User can set another photo as main
	Given I am logged in as default user
	When I go to edition of My Profile via button in right navigation menu
	And I set another photo as main
	Then I see main photo updated