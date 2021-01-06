using FluentAssertions;
using FluentAssertions.Execution;
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
    public sealed class FilteringSteps : BaseStep
    {

        private readonly ScenarioContext _scenarioContext;
        private PetsPage PetsPage;
        private string gender;
        private string type;

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

        [When(@"I filter pets by Type ""(.*)""")]
        public void WhenIFilterPetsByType(string type)
        {
            this.type = type;
            PetsPage.FilterPets(type: type);
        }


        [Then(@"I see only pets with given gender")]
        public void ThenISeeOnlyPetsWithGivenGender()
        {
            var pets = PetsPage.GetPetsFromCards();
            var petsFromDBWithGivenGender = SqlHelper.Pets.GetPetsByCriterium("Gender", gender).Count;
            using (new AssertionScope())
            {
                VerifyResultsGender(pets.Select(pet => pet.Name).ToList(), gender).Should().BeTrue($"All pets taken from UI should be of given gender '{gender}'");
                pets.Count.Should().Be(petsFromDBWithGivenGender, $"The number of retrieved cards from UI should be equal to number of pets in DB with given gender '{gender}'");
            } 
        }

        [Then(@"I see only pets with given type")]
        public void ThenISeeOnlyPetsWithGivenType()
        {
            var pets = PetsPage.GetPetsFromCards();
            var petsFromDBWithGivenType = SqlHelper.Pets.GetPetsByCriterium("Type", type).Count;
            using (new AssertionScope())
            {
                VerifyResultsType(pets.Select(pet => pet.Name).ToList(), type).Should().BeTrue($"All pets taken from UI should be of given type '{type}'");
                pets.Count.Should().Be(petsFromDBWithGivenType, $"The number of retrieved cards from UI should be equal to number of pets in DB with given type '{type}'");
            }
        }

        private bool VerifyResultsGender(List<string> petsNames, string gender)
        {
            var petsFromDb = new List<Pet>();
            foreach (var name in petsNames)
            {
                petsFromDb.Add(SqlHelper.Pets.GetPetByName(name));
            }
            return petsFromDb.All(pet => pet.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
        }

        private bool VerifyResultsType(List<string> petsNames, string type)
        {
            var petsFromDb = new List<Pet>();
            foreach (var name in petsNames)
            {
                petsFromDb.Add(SqlHelper.Pets.GetPetByName(name));
            }
            return petsFromDb.All(pet => pet.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

    }
}
