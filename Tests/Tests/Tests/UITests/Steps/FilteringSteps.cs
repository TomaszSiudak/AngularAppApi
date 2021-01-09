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
            pet = SqlHelper.Pets.GetPetByName(Variables.DefaultPet.Name);
            _scenarioContext.Add("pet", pet);
            PetsPage.AuthenticatePet(pet);
            PetsPage.NavigateToPetsPage();
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

        [When(@"I remove applied filter")]
        public void WhenIRemoveAppliedFilter()
        {
            PetsPage.RemoveFilter();
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

        [Then(@"I see only pets with given gender and type")]
        public void ThenISeeOnlyPetsWithGivenGenderAndType()
        {
            var pets = PetsPage.GetPetsFromCards();
            var petsFromDBWithGivenType = SqlHelper.Pets.GetPetsByCriterium("Type", type).Where(pet => pet.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase)).Count();
            using (new AssertionScope())
            {
                VerifyResultsGenderAndType(pets.Select(pet => pet.Name).ToList(), gender, type).Should().BeTrue($"All pets taken from UI should be of given gender '{gender}' and type '{type}'");
                pets.Count.Should().Be(petsFromDBWithGivenType, $"The number of retrieved cards from UI should be equal to number of pets in DB with given gender '{gender}' and type '{type}'");
            }
        }

        [Then(@"I see all pets")]
        public void ThenISeeAllPets()
        {
            var petsFromUI = PetsPage.GetPetsFromCards();
            var petsFromDB = SqlHelper.Pets.GetPets().Count;
            petsFromUI.Count.Should().Be(petsFromDB, $"The number of retrieved cards from UI should be equal to number of pets in DB after resetting filter");

        }



        private bool VerifyResultsGender(List<string> petsNames, string gender)
        {
            var petsFromDb = GetPetsByNameFromDB(petsNames);
            return petsFromDb.All(pet => pet.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
        }

        private bool VerifyResultsType(List<string> petsNames, string type)
        {
            var petsFromDb = GetPetsByNameFromDB(petsNames);
            return petsFromDb.All(pet => pet.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        private bool VerifyResultsGenderAndType(List<string> petsNames, string gender, string type)
        {
            var petsFromDb = GetPetsByNameFromDB(petsNames);
            return petsFromDb.All(pet => pet.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) && pet.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<Pet> GetPetsByNameFromDB(List<string> petsNames)
        {
            foreach (var name in petsNames)
            {
                yield return SqlHelper.Pets.GetPetByName(name);
            }
        }

    }
}
