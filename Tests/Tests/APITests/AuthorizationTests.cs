using FluentAssertions;
using Framework.Base;
using Framework.Constants;
using Framework.Extensions;
using Framework.Helpers;
using Framework.Helpers.SqlHelper;
using Framework.Models;
using NUnit.Framework;
using System;
using Tests.Base;

namespace Tests
{
    [TestFixture]
    public class AuthorizationTests : BaseAPITest
    {
        [Test]
        public void CorrectLoginTest()
        {
            Pet pet = new Pet() { Name = "Tom", Password = "test" };

            var response = AuthorizationAPI.Login(pet);

            var content = response.GetJObjectFromResponse();
            string token = content.Value<string>("token");
            int actualStatusCode = (int) response.StatusCode;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(200, actualStatusCode, $"Expected status code {200}, but was {actualStatusCode}");
                Assert.IsTrue(!string.IsNullOrEmpty(token), "Retrieved token cannot be null");
            });
        }

        [Test]
        public void LoginWithWrongPasswordTest()
        {
            Pet pet = new Pet() { Name = "Tom", Password = "test123qaz" };

            var response = AuthorizationAPI.Login(pet);

            var actualMessage = response.Content.Trim('"');
            int actualStatusCode = (int)response.StatusCode;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(401, actualStatusCode, $"Expected status code {401}, but was {actualStatusCode}");
                Assert.AreEqual(Messages.IncorrectLoginOrPassword, actualMessage);
            });
        }

        [Test]
        public void LoginWithNotExistingPetTest()
        {
            Pet pet = new Pet() { Name = "Tomek", Password = "test" };

            var response = AuthorizationAPI.Login(pet);

            var actualMessage = response.Content.Trim('"');
            int actualStatusCode = (int)response.StatusCode;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(401, actualStatusCode, $"Expected status code {401}, but was {actualStatusCode}");
                Assert.AreEqual(Messages.IncorrectLoginOrPassword, actualMessage);
            });
        }

        [Test]
        public void RegisterPetTest()
        {
            Pet expectedPet = new Pet() { Name = "Tom_" + StringHelper.GenerateRandomNumberString(4), Password = "test123", Age = 12, Gender = "male", City = "Katowice" };

            var response = AuthorizationAPI.Register(expectedPet);

            var registeredPet = response.DeserializeResponse<Pet>();
            int actualStatusCode = (int)response.StatusCode;
            bool wasPetAddedToDb = SqlHelper.Pets.IsPetInDb(expectedPet.Name);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(201, actualStatusCode, $"Expected status code {201}, but was {actualStatusCode}");
                registeredPet.Should().BeEquivalentTo(registeredPet, options => options.Excluding(p => p.Password), "The returned object should be equivalent to expected excluding password");
                Assert.IsTrue(string.IsNullOrEmpty(registeredPet.Password), "Password field of returned object from registration should be empty");
                Assert.IsTrue(wasPetAddedToDb, "Newly registered Pet was not added to Sql Server Db");
            });
        }

        [Test]
        public void TryRegisterPetWithExistingNameTest()
        {
            string existingName = "Tom";
            Pet expectedPet = new Pet() { Name = existingName, Password = "test123", Age = 12, Gender = "male", City = "Katowice" };

            var response = AuthorizationAPI.Register(expectedPet);

            var actualMessage = response.Content.Trim('"');
            int actualStatusCode = (int)response.StatusCode;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(400, actualStatusCode, $"Expected status code {400}, but was {actualStatusCode}");
                Assert.AreEqual(Messages.PetAlreadyExists, actualMessage);
            });
        }
    }
}
