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

        /*[Given(@"the pet data is prepared")]
        public void GivenThePetDataIsPrepared()
        {
            pet = new Pet() { Name = $"NewUser{StringHelper.GenerateRandomNumberString(3)}", Password = "test", Age = 1, City = "Rzym", Gender = "Female", Type = "Kot" };
        }*/

        [Given(@"the pet data is prepared Username = ""(.*)"", Password = ""(.*)"", ConfirmPassword = ""(.*)"", City = ""(.*)"", Gender = ""(.*)"", Type = ""(.*)""")]
        public void GivenThePetDataIsPreparedUsernamePasswordConfirmPasswordCityGenderType(string username, string pass, string confirmPass, string city, string gender, string type)
        {
            pet = new Pet() { Name = $"{username}{StringHelper.GenerateRandomNumberString(3)}", Password = pass, ConfirmPassword = confirmPass, Age = 1, City = city, Gender = gender, Type = type };
        }

        [When(@"I fill the form")]
        public void WhenIFillTheForm()
        {
            MainPage.FillTheRegistrationForm(pet);
        }

        [When(@"I click register btn")]
        public void WhenIClickRegisterBtn()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the pet is registered and toast message is visible")]
        public void ThenThePetIsRegisteredAndToastMessageIsVisible()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I am able to log in")]
        public void ThenIAmAbleToLogIn()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
