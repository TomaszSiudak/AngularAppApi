Feature: Registration
	In order to use full features of the Petbook app
	As a future user
	I must to be able to create an account


Scenario: Register Pet
	Given the registration form is opened
	And the pet data is prepared Username = "NewUser", Password = "test", ConfirmPassword = "test", City = "Rzym", Gender = "female", Type = "Kot"
	When I fill the form
	And I register pet
	Then the pet is registered and user is informed
	And I am able to log in


Scenario: Try Register Pet with existing name
	Given the registration form is opened
	And the pet data is prepared Username = "Tom", Password = "test", ConfirmPassword = "test", City = "Londyn", Gender = "male", Type = "Pies"
	When I fill the form
	And I try to register pet
	Then the pet is NOT registered and user is informed


Scenario: Try Register Pet without name
	Given the registration form is opened
	And the pet data is prepared Username = "", Password = "test", ConfirmPassword = "test", City = "Londyn", Gender = "male", Type = "Królik"
	When I fill the form
	And I try to register pet
	Then the pet is NOT registered and hint "Nazwa użytkownika nie może być pusta" is present


Scenario: Try Register Pet not matching passwords
	Given the registration form is opened
	And the pet data is prepared Username = "NewUser", Password = "Test123", ConfirmPassword = "Test12", City = "Londyn", Gender = "male", Type = "Chomik"
	When I fill the form
	And I try to register pet
	Then the pet is NOT registered and hint "Potwierdzenie hasła jest inne niż powyżej. Sprawdź ponownie." is present


Scenario: Try Register Pet with too short password
	Given the registration form is opened
	And the pet data is prepared Username = "NewUser", Password = "Abc", ConfirmPassword = "Abc", City = "Londyn", Gender = "male", Type = "Papuga"
	When I fill the form
	And I try to register pet
	Then the pet is NOT registered and hint "Hasło musi zawierać co najmniej 4 znaki" is present