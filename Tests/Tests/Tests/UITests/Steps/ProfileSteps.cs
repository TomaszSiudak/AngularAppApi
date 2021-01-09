using FluentAssertions;
using FluentAssertions.Execution;
using Framework.API;
using Framework.Constants;
using Framework.Helpers;
using Framework.Models;
using NUnit.Framework;
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
    public sealed class ProfileSteps : BaseStep
    {
        private readonly ScenarioContext scenarioContext;

        public ProfileSteps(ScenarioContext scenarioContext, IWebDriver driver, MainPage mainPage, ProfilePage profilePage)
        {
            this.scenarioContext = scenarioContext;
            Driver = driver;
            MainPage = mainPage;
            ProfilePage = profilePage;
        }

        [Given(@"I am new user")]
        public void GivenIAmNewUser()
        {
            pet = new Pet() { Name = "NewTom_" + StringHelper.GenerateRandomNumberString(4), Password = "test", Type = "Dog", Gender = "male" };
            new AuthorizationAPI().AddPet(pet);
        }

        [When(@"I go to My Profile")]
        public void WhenIGoToMyProfile()
        {
            ProfilePage = MainPage.GoToMyProfile();
        }

        [Given(@"I am at my current account")]
        public void GivenIAmAtMyCurrentAccount()
        {
            MainPage.Login(pet.Name, pet.Password).GoToMyProfile();
        }

        [When(@"I go to edition of My Profile")]
        public void WhenIGoToEditionOfMyProfile()
        {
            EditProfilePage = ProfilePage.GoToEditPage();
        }

        [When(@"I edit pet fields Username = ""(.*)"", Age = (.*), City = ""(.*)"", Gender = ""(.*)"", Description = ""(.*)""")]
        public void WhenIEditPetFieldsUsernameAgeCityGenderDescription(string name, int age, string city, string gender, string desc)
        {
            string petname = string.IsNullOrEmpty(name) ? name : $"{name}{StringHelper.GenerateRandomNumberString(3)}";
            var dataForPetEdition = new Pet() { Name = petname, Age = age, Gender = gender, City = city, Description = desc };
            scenarioContext.Add("pet", dataForPetEdition);
            EditProfilePage.EditPet(dataForPetEdition);
        }

        [When(@"I save current changes")]
        public void WhenISaveCurrentChanges()
        {
            var toastMessage = EditProfilePage.SaveEdition();
            scenarioContext.Add("toast", toastMessage);
        }


        [Then(@"I see my account data")]
        public void ThenISeeMyAccountData()
        {
            pet = scenarioContext["pet"] as Pet;
            string expectedHeader = $"Profil {pet.Name}";
            var petFromUI = ProfilePage.GetPetFromProfile();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedHeader, ProfilePage.GetPetProfileNameHeader(), "Profile header should match expected name");
                Assert.AreEqual(pet.Name, petFromUI.Name, "Actual profile Name value should match expected");
                Assert.AreEqual(pet.Age, petFromUI.Age, "Actual profile Age value should match expected");
                Assert.AreEqual(pet.Gender, petFromUI.Gender, "Actual profile Gender value should match expected");
                Assert.AreEqual(pet.City, petFromUI.City, "Actual profile City value should match expected");
                Assert.IsTrue(pet.Description.Contains(petFromUI.Description), "Actual profile Description value should match expected");
            });
        }


    }
}
