Feature: Registration
	In order to use full features of the Petbook app
	As a future user
	I must to be able to create an account

@mytag
Scenario: Register Pet
	Given the registration form is opened
	And the pet data is prepared Username = "NewUser", Password = "test", ConfirmPassword = "test", City = "Rzym", Gender = "female", Type = "Kot"
	When I fill the form
	And I click register btn
	Then the pet is registered and toast message is visible
	And I am able to log in