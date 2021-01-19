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
    [Category("API")]
    class PetsTests : BaseAPITest
    {
        [Test]
        public void AddLikeToPetTest()
        {
            Pet existingPet = Variables.DefaultPet;
            Pet newPet = new Pet() { Name = "AddLikePet_" + StringHelper.GenerateRandomNumberString(6), Password = "test123", City = "Katowice" };
            AuthorizationAPI.AddPet(newPet);
            var newPetFromDb = SqlHelper.Pets.GetPetByName(newPet.Name);
            var existingPetFromDb = SqlHelper.Pets.GetPetByName(existingPet.Name);

            var response = PetsAPI.AddLikeToPet(newPetFromDb.Id, likerId: existingPetFromDb.Id, petToAuthorize: existingPet);

            int actualStatusCode = (int)response.StatusCode;
            var petLikesIds = SqlHelper.Likes.GetLikesOfPetById(newPetFromDb.Id);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(200, actualStatusCode, $"Expected status code {200}, but was {actualStatusCode}");
                Assert.IsEmpty(response.Content, $"No content should be returned when adding like to pet");
                Assert.AreEqual(1, petLikesIds.Count, "Only one like should be added");
                Assert.AreEqual(existingPetFromDb.Id, petLikesIds.FirstOrDefault(), "Like should be added from expected pet and match its Id");
            });
        }

        [Test]
        public void AddSeveralLikesToPetTest()
        {
            Pet existingPet = Variables.DefaultPet;
            Pet newPet = new Pet() { Name = "AddFewLikesPet_" + StringHelper.GenerateRandomNumberString(6), Password = "test123", City = "Katowice" };
            AuthorizationAPI.AddPet(newPet);
            var newPetFromDb = SqlHelper.Pets.GetPetByName(newPet.Name);
            var existingPetFromDb = SqlHelper.Pets.GetPetByName(existingPet.Name);

            PetsAPI.AddLikeToPet(newPetFromDb.Id, likerId: existingPetFromDb.Id, petToAuthorize: existingPet);
            PetsAPI.AddLikeToPet(newPetFromDb.Id, likerId: existingPetFromDb.Id, petToAuthorize: existingPet);
            var response = PetsAPI.AddLikeToPet(newPetFromDb.Id, existingPetFromDb.Id, petToAuthorize: existingPet);

            int actualStatusCode = (int)response.StatusCode;
            var petLikesIds = SqlHelper.Likes.GetLikesOfPetById(newPetFromDb.Id);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(400, actualStatusCode, $"Expected status code {400}, but was {actualStatusCode}");
                Assert.AreEqual(Messages.PetWasAlreadyLiked, response.Content.Trim('"'), $"Returned response content should match expected message");
                Assert.AreEqual(1, petLikesIds.Count, "Only one like should be added from existing pet");
            });
        }

        [Test]
        public void CheckGetLikesFromPetEndpointTest()
        {
            Pet testPetWithId4 = SqlHelper.Pets.GetPetById(4);
            Pet petWithId2 = SqlHelper.Pets.GetPetById(2);
            PetsAPI.AddLikeToPet(testPetWithId4.Id, likerId: petWithId2.Id, petWithId2);

            var response = PetsAPI.GetPetsWhichLikedCurrentProfile(testPetWithId4.Id, petToAuthorize: testPetWithId4);

            var retrievedPets = response.DeserializeResponse<List<Pet>>();
            Pet petWithId2FromResponse = retrievedPets.FirstOrDefault(pet => pet.Name.Equals(petWithId2.Name));
            int actualStatusCode = (int)response.StatusCode;
            var expectedNumberOfPetsWhichLikedCurrentProfile = SqlHelper.Likes.GetLikesOfPetById(testPetWithId4.Id).Count;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(200, actualStatusCode, $"Expected status code {200}, but was {actualStatusCode}");
                petWithId2.Should().BeEquivalentTo(petWithId2FromResponse, options => options.Excluding(p => p.Password), "The Pet returned from API response should be equivalent to expected pet's data from DB excluding password");
                Assert.AreEqual(expectedNumberOfPetsWhichLikedCurrentProfile, retrievedPets.Count, $"Expected number of likes should match those from API response");
            });
        }

        [Test]
        public void CheckGetPetEndpointTest()
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
        public void GetPetWithNotExistingIdTest()
        {
            int notExistingId = 12345678;
            var response = PetsAPI.GetPetById(notExistingId);

            int actualStatusCode = (int)response.StatusCode;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(204, actualStatusCode, $"Expected status code {204}, but was {actualStatusCode}");
                Assert.IsEmpty(response.Content, $"No content should be present when getting pet with not existing Id");
            });
        }


        [Test]
        public void CheckGetPetsEndpointTest()
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


        [Test]
        public void UpdateExistingPetTest()
        {
            Pet pet = new Pet() { Name = "Tom_" + StringHelper.GenerateRandomNumberString(6), Password = "test123", Age = 5, Gender = "male", City = "Katowice" };
            AuthorizationAPI.AddPet(pet);
            var petFromDbBeforeEdit = SqlHelper.Pets.GetPetByName(pet.Name);
            petFromDbBeforeEdit.Password = pet.Password;
            Pet dataToEdit = new Pet() {Name = pet.Name, Age = 6, Gender = pet.Gender, City = "Bytom", Description = "Super pies" };

            var response = PetsAPI.UpdatePet(dataToEdit, petFromDbBeforeEdit.Id, petFromDbBeforeEdit);
            var updatedPet = response.DeserializeResponse<Pet>();

            int actualStatusCode = (int)response.StatusCode;
            var petFromDbAfterEdit = SqlHelper.Pets.GetPetByName(pet.Name);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(201, actualStatusCode, $"Expected status code {201}, but was {actualStatusCode}");
                petFromDbAfterEdit.Should().BeEquivalentTo(dataToEdit, options => options.Excluding(p => p.Password).Excluding(p => p.Type).Excluding(p => p.Id).Excluding(p => p.Photos), "The Pet returned from DB should be equivalent to expected data excluding password, Id and Type");
                Assert.AreEqual(petFromDbBeforeEdit.Name, updatedPet.Name, "The returned Pet from request should have the same Name as initial Pet from Db");
            });
        }

        [Test]
        public void UpdateExistingPetWithAllValuesTest()
        {
            Pet pet = new Pet() { Name = "PetAllValues_" + StringHelper.GenerateRandomNumberString(4), Password = "test123" };
            AuthorizationAPI.AddPet(pet);
            var petFromDbBeforeEdit = SqlHelper.Pets.GetPetByName(pet.Name);
            petFromDbBeforeEdit.Password = pet.Password;
            Pet dataToEdit = new Pet() { Name = pet.Name, Age = 10, Gender = "female", City = "Katowice", Description = "Super kot", Type = "cat" };

            var response = PetsAPI.UpdatePet(dataToEdit, petFromDbBeforeEdit.Id, petFromDbBeforeEdit);
            var updatedPet = response.DeserializeResponse<Pet>();

            int actualStatusCode = (int)response.StatusCode;
            var petFromDbAfterEdit = SqlHelper.Pets.GetPetByName(pet.Name);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(201, actualStatusCode, $"Expected status code {201}, but was {actualStatusCode}");
                petFromDbAfterEdit.Should().BeEquivalentTo(dataToEdit, options => options.Excluding(p => p.Password).Excluding(p => p.Type).Excluding(p => p.Id).Excluding(p => p.Photos), "The Pet returned from DB should be equivalent to expected data excluding password, Id and Type");
                Assert.AreEqual(petFromDbBeforeEdit.Name, updatedPet.Name, "The returned Pet from request should have the same Name as initial Pet from Db");
            });
        }
    }
}
