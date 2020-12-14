using FluentAssertions;
using Framework.API;
using Framework.Base;
using Framework.Constants;
using Framework.Extensions;
using Framework.Helpers;
using Framework.Models;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.Base;

namespace Tests.APITests
{
    [TestFixture]
    class PetsTests : BaseAPITest
    {
        [Test]
        public void CheckGetPetEnpointTest()
        {
            Pet expectedPet = SqlHelper.Pets.GetRandomPet();

            var response = PetsAPI.GetPetById(expectedPet.Id);

            var retrievedPet = response.DeserializeResponse<Pet>();
            int actualStatusCode = (int)response.StatusCode;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(200, actualStatusCode, $"Expected status code {200}, but was {actualStatusCode}");
                Assert.AreEqual(expectedPet.Name, retrievedPet.Name, $"Expected Name {expectedPet.Name}, but was {retrievedPet.Name}");
                Assert.AreEqual(expectedPet.Age, retrievedPet.Age, $"Expected Age {expectedPet.Age}, but was {retrievedPet.Age}");
                Assert.AreEqual(expectedPet.Gender, retrievedPet.Gender, $"Expected Gender {expectedPet.Gender}, but was {retrievedPet.Gender}");
            });
        }


        [Test]
        public void CheckGetPetsEnpointTest()
        {
            var expectedPets = SqlHelper.Pets.GetPets();
            var parameters = new PetQueryParameters() { currentPage = 1, pageSize = 10000 };

            var response = PetsAPI.GetPetsBySearchCriteria(parameters);

            var retrievedPets = response.DeserializeResponse<List<Pet>>();
            int actualStatusCode = (int)response.StatusCode;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(200, actualStatusCode, $"Expected status code {200}, but was {actualStatusCode}");
                Assert.AreEqual(expectedPets.Count, retrievedPets.Count, $"Expected number of retrieved pets should equal those in DB");
                CollectionAssert.AreEquivalent(expectedPets.Select(pet => pet.Name), retrievedPets.Select(pet => pet.Name), $"Expected names of retrieved pets should be equivalent to those in DB");
            });
        }

        [Test]
        public void GetPetsByGenderTest()
        {
            string expectedGender = "male";
            var expectedPets = SqlHelper.Pets.GetPetsByCriterium("Gender", expectedGender);
            var parameters = new PetQueryParameters() { currentPage = 1, pageSize = 10000, gender = expectedGender };

            var response = PetsAPI.GetPetsBySearchCriteria(parameters);

            var retrievedPets = response.DeserializeResponse<List<Pet>>();
            int actualStatusCode = (int)response.StatusCode;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(200, actualStatusCode, $"Expected status code {200}, but was {actualStatusCode}");
                Assert.AreEqual(expectedPets.Count, retrievedPets.Count, $"Expected number of retrieved pets should equal those in DB");
                Assert.IsTrue(retrievedPets.All(pet => pet.Gender.Equals(expectedGender)), $"All retrieved pets should be of searched gender {expectedGender}");
            });
        }

        [Test]
        public void GetPetsByTypeTest()
        {
            string expectedType= "cat";
            var expectedPets = SqlHelper.Pets.GetPetsByCriterium("Type", expectedType);
            var parameters = new PetQueryParameters() { currentPage = 1, pageSize = 10000, type = expectedType };

            var response = PetsAPI.GetPetsBySearchCriteria(parameters);

            var retrievedPets = response.DeserializeResponse<List<Pet>>();
            int actualStatusCode = (int)response.StatusCode;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(200, actualStatusCode, $"Expected status code {200}, but was {actualStatusCode}");
                Assert.AreEqual(expectedPets.Count, retrievedPets.Count, $"Expected number of retrieved pets should equal those in DB");
                Assert.IsTrue(retrievedPets.All(pet => pet.Type.Equals(expectedType)), $"All retrieved pets should be of searched type {expectedType}");
            });
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(12)]
        public void GetPetsWithSpecificPageSizeTest(int pageSize)
        {
            var parameters = new PetQueryParameters() { currentPage = 1, pageSize = pageSize };

            var response = PetsAPI.GetPetsBySearchCriteria(parameters);

            var retrievedPets = response.DeserializeResponse<List<Pet>>();
            int actualStatusCode = (int)response.StatusCode;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(200, actualStatusCode, $"Expected status code {200}, but was {actualStatusCode}");
                Assert.AreEqual(pageSize, retrievedPets.Count, $"Expected number of retrieved pets should equal {pageSize}, but was {retrievedPets.Count}");
            });
        }
    }
}
