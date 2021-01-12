using FluentAssertions;
using FluentAssertions.Execution;
using Framework.Constants;
using Framework.Helpers.SqlHelper;
using Framework.Models;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Tests.Base;
using Tests.Pages;
using Tests.Pages.PagesElements;

namespace Tests.Tests.UITests.Steps
{
    [Binding]
    public sealed class LoginSteps : BaseStep
    {
        private readonly ScenarioContext scenarioContext;

        public LoginSteps(ScenarioContext scenarioContext, IWebDriver driver, MainPage mainPage)
        {
            this.scenarioContext = scenarioContext;
            Driver = driver;
            MainPage = mainPage;
        }

        [Given(@"I am registered user")]
        public void GivenIAmRegisteredUser()
        {
            pet = SqlHelper.Pets.GetRandomPet();
        }

        [Given(@"the username does not exist")]
        public void GivenTheUsernameDoesNotExist()
        {
            pet = new Pet() { Name = "NotExistingUser", Password = "test123" };
        }

        [Given(@"the user uses incorrect password")]
        public void GivenTheUserUsesIncorrectPassword()
        {
            pet = SqlHelper.Pets.GetRandomPet();
            pet.Password = "NotExistingPassword";
        }

        [When(@"I login to application")]
        public void WhenILoginToApplication()
        {
            PetsPage = MainPage.Login(pet.Name, pet.Password);
            scenarioContext.Add("toast", MainPage.GetToastMessage());
        }

        [When(@"I try login to application")]
        public void WhenITryLoginToApplication()
        {
            MainPage.TryToLogin(pet.Name, pet.Password);
            scenarioContext.Add("toast", MainPage.GetToastMessage());
        }


        [Then(@"the photos page and personal links are visible")]
        public void ThenThePhotosPageAndPersonalLinksAreVisible()
        {
            using (new AssertionScope())
            {
                PetsPage.IsElementVisible(PetsPageElements.PetsPageHeaderBy).Should().BeTrue("User should redirected after login and photos page header should be visible");
                PetsPage.IsElementVisible(NavigationMenuElements.MyProfileBy).Should().BeTrue("'Mój profil' link should be visible after login");
                PetsPage.IsElementVisible(NavigationMenuElements.RightMenuBtnBy).Should().BeTrue("'Moje konto' btn should be visible after login");
                scenarioContext["toast"].Should().BeEquivalentTo(Messages.LoginSuccessfull);
            }
            //Assert.IsTrue(PhotosPage.PhotosPageElements.PhotosPageHeader.IsVisible(), "Photos page header should be visible");
        }


        [Then(@"the user is not redirected and toast message is visible")]
        public void ThenTheUserIsNotRedirectedAndToastMessageIsVisible()
        {
            using (new AssertionScope())
            {
                MainPage.IsElementVisible(MainPageElements.MainPageHeaderBy).Should().BeTrue("User should NOT be redirected after trying to login with incorrect credentials");
                MainPage.IsElementVisible(NavigationMenuElements.MyProfileBy).Should().BeFalse("'Mój profil' link should NOT be visible after trying to login with incorrect credentials");
                MainPage.IsElementVisible(NavigationMenuElements.RightMenuBtnBy).Should().BeFalse("'Moje konto' btn should NOT be visible after trying to login with incorrect credentials");
                scenarioContext["toast"].Should().BeEquivalentTo(Messages.IncorrectLoginOrPassword);
            }
        }

    }
}
