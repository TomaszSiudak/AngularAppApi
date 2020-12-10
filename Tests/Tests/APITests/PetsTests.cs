using FluentAssertions;
using Framework.Base;
using Framework.Constants;
using Framework.Extensions;
using Framework.Helpers;
using Framework.Models;
using NUnit.Framework;
using System;
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
    }
}
