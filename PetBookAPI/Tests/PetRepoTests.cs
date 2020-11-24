using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using NUnit.Framework;
using PetBookAPI.Controllers;
using PetBookAPI.DataTransferFiles;
using PetBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class PetRepoTests
    {
        List<Pet> loginTestData = new List<Pet>
        { 
            new Pet() { Name = "Tomek", Password = "test!123" }, 
            new Pet() { Name = "Andrew", Password = "" }
        };
        
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetPhotoByIdTest()
        {
            //Arrange
            var dbContextMock = new Mock<Context>();
            var dbSetMock = new Mock<DbSet<Photo>>();
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new Photo() {Description = "TestPhoto", Url = "www.test.com.pl" }));
            dbContextMock.Setup(s => s.Photos).Returns(dbSetMock.Object); 

            //Act
            var petRepository = new PetRepository(dbContextMock.Object);
            var photo = petRepository.GetPhoto(new Random().Next(1, 100)).Result;

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Photos);
                dbSetMock.Verify(x => x.FindAsync(It.IsAny<int>()));
                Assert.IsNotNull(photo, "Retrieved object cannot be null");
                Assert.AreEqual("TestPhoto", photo.Description, "Expected description should match actual");
                Assert.AreEqual("www.test.com.pl", photo.Url, "Expected description should match actual");
            });
        }

        [Test]
        public void VerifyLoginWithCorrectDataTest()
        {
            //Arrange
            string username = "Tomek";
            string password = "test!123";
            var dbContextMock = new Mock<Context>();
            var dbSetMock = GetMockDbSet<Pet>(loginTestData);
            dbContextMock.Setup(s => s.Pets).Returns(dbSetMock.Object);
 
            //Act
            var authRepo = new AuthorizationRepo(dbContextMock.Object);
            var pet = authRepo.Login(username, password).Result;

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Pets);
                Assert.IsNotNull(pet, "Retrieved object cannot be null");
                Assert.AreEqual(username, pet.Name, "Expected description should match actual");
                Assert.AreEqual(password, pet.Password, "Expected description should match actual");
            });
        }

        [Test]
        public void VerifyLoginWithNotExistingNameTest()
        {
            //Arrange
            string username = "NotExistingName";
            string password = "test!123";
            var dbContextMock = new Mock<Context>();
            var dbSetMock = GetMockDbSet<Pet>(loginTestData);
            dbContextMock.Setup(s => s.Pets).Returns(dbSetMock.Object);

            //Act
            var authRepo = new AuthorizationRepo(dbContextMock.Object);
            var pet = authRepo.Login(username, password).Result;

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Pets);
                Assert.IsNull(pet, "Retrieved object should be null if username does not exists");
            });
        }

        [Test]
        public void VerifyLoginWithNotCorrectPasswordTest()
        {
            //Arrange
            string username = "Tomek";
            string password = "test";
            var dbContextMock = new Mock<Context>();
            var dbSetMock = GetMockDbSet<Pet>(loginTestData);
            dbContextMock.Setup(s => s.Pets).Returns(dbSetMock.Object);

            //Act
            var authRepo = new AuthorizationRepo(dbContextMock.Object);
            var pet = authRepo.Login(username, password).Result;

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Pets);
                Assert.IsNull(pet, "Retrieved object should be null if password is not correct");
            });
        }


        [Test]
        public void RegisterPetTest()
        {
            //Arrange 
            var testPet = new Pet() { Name = "Tom" };
            var dbContextMock = new Mock<Context>();
            var dbSetMock = new Mock<DbSet<Pet>>(MockBehavior.Loose);
            dbContextMock.Setup(s => s.Pets).Returns(dbSetMock.Object);

            //Act
            var authRepo = new AuthorizationRepo(dbContextMock.Object);
            var pet = authRepo.Register(testPet).Result;

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Pets);
                dbSetMock.Verify(x => x.AddAsync(testPet, default));
                dbContextMock.Verify(x => x.SaveChangesAsync(default));
                Assert.IsNotNull(pet, "Retrieved object cannot be null");
                Assert.AreEqual("Tom", pet.Name);
            });
        }


        internal Mock<DbSet<T>> GetMockDbSet<T>(ICollection<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(entities.Add);
            return mockSet;
        }
    }
}