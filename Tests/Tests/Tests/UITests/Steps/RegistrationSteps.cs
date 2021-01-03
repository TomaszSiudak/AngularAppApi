using FluentAssertions;
using FluentAssertions.Execution;
using Framework.Constants;
using Framework.Helpers;
using Framework.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Tests.Base;
using Tests.Pages;
using Tests.Pages.PagesElements;

namespace Tests.Tests.UITests.Steps
{
    [Binding]
    public sealed class RegistrationSteps : BaseStep
    {
        private readonly ScenarioContext scenarioContext;
        private Pet pet;
        private MainPage MainPage;

        public RegistrationSteps(ScenarioContext scenarioContext, IWebDriver driver, MainPage mainPage)
        {
            this.scenarioContext = scenarioContext;
            Driver = driver;
            MainPage = mainPage;
            MainPage.GoToMainPage();
        }

        [Given(@"the registration form is opened")]
        public void GivenTheRegistrationFormIsOpened()
        {
            MainPage.OpenRegistrationForm();
        }

        [Given(@"the pet data is prepared Username = ""(.*)"", Password = ""(.*)"", ConfirmPassword = ""(.*)"", City = ""(.*)"", Gender = ""(.*)"", Type = ""(.*)""")]
        public void GivenThePetDataIsPreparedUsernamePasswordConfirmPasswordCityGenderType(string petName, string pass, string confirmPass, string city, string gender, string type)
        {
            string name = petName.Equals(Variables.DefaultPet.Name) || string.IsNullOrEmpty(petName) ? petName : $"{petName}{StringHelper.GenerateRandomNumberString(3)}";
            pet = new Pet() { Name = name, Password = pass, ConfirmPassword = confirmPass, Age = 1, City = city, Gender = gender, Type = type };
        }

        [When(@"I fill the form")]
        public void WhenIFillTheForm()
        {
            MainPage.FillTheRegistrationForm(pet);
        }

        [When(@"I register pet")]
        public void WhenIRegisterPet()
        {
            MainPage.RegisterPet();
        }

        [When(@"I try to register pet")]
        public void WhenITryToRegisterPet()
        {
            MainPage.TryRegisterPet();
        }


        [Then(@"the pet is registered and user is informed")]
        public void ThenThePetIsRegisteredAndUserIsInformed()
        {
            using (new AssertionScope())
            {
                MainPage.IsElementVisible(MainPageElements.RegistrationFormBy).Should().BeFalse("Registration form should be closed after successfull registration");
                SqlHelper.Pets.IsPetInDb(pet.Name).Should().BeTrue("The newly created pet should be stored into DB");
                MainPage.GetToastMessage().Should().BeEquivalentTo(Messages.RegistrationSuccessfull);
            }
        }

        [Then(@"I am able to log in")]
        public void ThenIAmAbleToLogIn()
        {
            var PhotosPage = MainPage.Login(pet.Name, pet.Password);
            PhotosPage.IsElementVisible(PhotosPageElements.PhotosPageHeaderBy).Should().BeTrue("User should redirected after login and photos page header should be visible");
        }

        [Then(@"the pet is NOT registered and user is informed")]
        public void ThenThePetIsNOTRegisteredAndUserIsInformed()
        {
            using (new AssertionScope())
            {
                MainPage.GetToastMessage().Should().BeEquivalentTo(Messages.PetAlreadyExists);
                MainPage.IsElementVisible(MainPageElements.RegistrationFormBy).Should().BeTrue("Registration form should be still visible after trying to register pet with existing name");
                SqlHelper.Pets.IsPetInDb(pet.Name).Should().BeTrue($"The pet with existing name {pet.Name} should be only one in DB");
            }
        }

        [Then(@"the pet is NOT registered and hint ""(.*)"" is present")]
        public void ThenThePetIsNOTRegisteredAndHintIsPresent(string hint)
        {
            using (new AssertionScope())
            {
                MainPage.GetRegistrationFormVisibleHintText().Should().BeEquivalentTo(hint);
                MainPage.IsElementVisible(MainPageElements.RegistrationFormBy).Should().BeTrue("Registration form should be still visible after trying to register pet with incorrect data");
                SqlHelper.Pets.IsPetInDb(pet.Name).Should().BeFalse($"The pet with inccorect data should NOT be stored into DB");
            }
        }



    }
}
