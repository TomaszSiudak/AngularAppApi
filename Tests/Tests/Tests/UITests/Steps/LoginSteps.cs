using BoDi;
using Framework.Helpers.SqlHelper;
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
    public sealed class LoginSteps : BaseStep
    {
        private Pet pet;
        private readonly ScenarioContext _scenarioContext;
        private readonly IObjectContainer objectContainer;

        public LoginSteps(ScenarioContext scenarioContext, IObjectContainer objectContainer, IWebDriver driver)
        {
            _scenarioContext = scenarioContext;
            this.objectContainer = objectContainer;
            Driver = driver;
            Driver = objectContainer.Resolve<IWebDriver>();
        }

        [Given(@"I am registered user")]
        public void GivenIAmRegisteredUser()
        {
            pet = SqlHelper.Pets.GetRandomPet();
            CurrentPage = (MainPage) new MainPage(Driver);
        }

        [When(@"I enter correct login and password")]
        public void WhenIEnterCorrectLoginAndPassword()
        {
            
        }

        [When(@"I click Zaloguj się button")]
        public void WhenIClickZalogujSieButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the photos page and personal links are visible")]
        public void ThenThePhotosPageAndPersonalLinksAreVisible()
        {
            ScenarioContext.Current.Pending();
        }


    }
}
