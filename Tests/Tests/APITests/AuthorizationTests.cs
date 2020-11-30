using Framework.Base;
using Framework.Extensions;
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
        public void VerifyCorrectLoginTest()
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
    }
}
