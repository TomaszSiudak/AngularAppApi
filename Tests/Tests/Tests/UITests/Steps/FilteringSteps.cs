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
    public sealed class FilteringSteps : BaseStep
    {

        private readonly ScenarioContext _scenarioContext;
        private PetsPage PetsPage;
        private string gender;

        public FilteringSteps(ScenarioContext scenarioContext, IWebDriver driver, PetsPage petsPage)
        {
            _scenarioContext = scenarioContext;
            Driver = driver;
            PetsPage = petsPage;
        }

        [Given(@"I am logged in at photos url")]
        public void GivenIAmLoggedInAtPhotosUrl()
        {
            PetsPage.AuthenticatePet(Variables.DefaultPet);
            PetsPage.GoToPhotosPage();
        }


        [When(@"I filter pets by Gender ""(.*)""")]
        public void WhenIFilterPetsByGender(string gender)
        {
            this.gender = gender;
            PetsPage.FilterPets(gender: gender);
        }


        [Then(@"I see only pets with given criteria")]
        public void ThenISeeOnlyPetsWithGivenCriteria()
        {
            int numberOfPets = PetsPage.PetsPageElements.PetCards.Count;
            var pets = SqlHelper.Pets.GetPetsByCriterium("Gender", gender).Count;
            numberOfPets.Should().Be(pets);

        }

    }
}
