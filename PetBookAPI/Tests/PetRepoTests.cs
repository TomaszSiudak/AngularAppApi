using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using PetBookAPI;
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

        List<Likes> likesTestData = new List<Likes>
        {
            new Likes() { Id = 1, Pet = new Pet(){ Name = "Rex" }, PetId = 1, PetWhichLikedId = 2 },
            new Likes() { Id = 2, Pet = new Pet(){ Name = "Andrew" }, PetId = 2, PetWhichLikedId = 1 },
            new Likes() { Id = 3, Pet = new Pet(){ Name = "Rex" }, PetId = 1, PetWhichLikedId = 3 },
            new Likes() { Id = 4, Pet = new Pet(){ Name = "Salazar" }, PetId = 2, PetWhichLikedId = 2 },
            new Likes() { Id = 5, Pet = new Pet(){ Name = "Rex" }, PetId = 1, PetWhichLikedId = 4 },
        };
        List<Pet> pagingTestData = new List<Pet>()
        {
            new Pet() { Name = "Tomek", Password = "test!123" },
            new Pet() { Name = "Andrew", Password = "" },
            new Pet() { Name = "Tomek", Password = "test!123" },
            new Pet() { Name = "Andrew", Password = "" },
            new Pet() { Name = "Tomek", Password = "test!123" },
            new Pet() { Name = "Andrew", Password = "" },
            new Pet() { Name = "Tomek", Password = "test!123" },
            new Pet() { Name = "Andrew", Password = "" },
            new Pet() { Name = "Tomek", Password = "test!123" },
            new Pet() { Name = "Andrew", Password = "" },
            new Pet() { Name = "Tomek", Password = "test!123" },
            new Pet() { Name = "Andrew", Password = "" },
            new Pet() { Name = "Tomek", Password = "test!123" }
        };


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
                dbSetMock.Verify(x => x.AddAsync(testPet, default), Times.Once);
                dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
                Assert.IsNotNull(pet, "Retrieved object cannot be null");
                Assert.AreEqual("Tom", pet.Name);
            });
        }

        [Test]
        public void VerifyPetExistsTest()
        {
            //Arrange
            string username = "Tomek";
            var dbContextMock = new Mock<Context>();
            var dbSetMock = GetMockDbSet<Pet>(loginTestData);
            dbContextMock.Setup(s => s.Pets).Returns(dbSetMock.Object);

            //Act
            var authRepo = new AuthorizationRepo(dbContextMock.Object);
            var petExists = authRepo.Exists(username);

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Pets, Times.Once);
                Assert.IsTrue(petExists, "Pet should exists");
            });
        }

        [Test]
        public void VerifyPetNotExistsTest()
        {
            //Arrange
            string username = "Tom";
            var dbContextMock = new Mock<Context>();
            var dbSetMock = GetMockDbSet<Pet>(loginTestData);
            dbContextMock.Setup(s => s.Pets).Returns(dbSetMock.Object);

            //Act
            var authRepo = new AuthorizationRepo(dbContextMock.Object);
            var petExists = authRepo.Exists(username);

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Pets, Times.Once);
                Assert.IsFalse(petExists, "Pet should not exists");
            });
        }

        [Test]
        public void VerifyAddPetWasCalledTest()
        {
            //Arrange
            var testPet = new Pet() { Name = "Tom", Age = 10, City = "Katowice", Gender = "male" };
            var dbContextMock = new Mock<Context>();
            var dbSetMock = GetMockDbSet<Pet>(loginTestData);

            //Act
            var petRepo = new PetRepository(dbContextMock.Object);
            petRepo.AddPet(testPet);

            //Assert
            dbContextMock.Verify(x => x.Add(testPet), Times.Once);
        }

        [Test]
        public void VerifyDeletePetWasCalledTest()
        {
            //Arrange
            var testPet = new Pet() { Name = "Tom", Age = 10, City = "Katowice", Gender = "male" };
            var dbContextMock = new Mock<Context>();

            //Act
            var petRepo = new PetRepository(dbContextMock.Object);
            petRepo.DeletePet(testPet);

            //Assert
            dbContextMock.Verify(x => x.Remove(testPet), Times.Once); ;
        }

        [Test]
        public void GetPhotoByIdTest()
        {
            //Arrange
            var dbContextMock = new Mock<Context>();
            var dbSetMock = new Mock<DbSet<Photo>>();
            dbSetMock.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new Photo() { Description = "TestPhoto", Url = "www.test.com.pl" }));
            dbContextMock.Setup(s => s.Photos).Returns(dbSetMock.Object);

            //Act
            var petRepository = new PetRepository(dbContextMock.Object);
            var photo = petRepository.GetPhoto(new Random().Next(1, 100)).Result;

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Photos, Times.Once);
                dbSetMock.Verify(x => x.FindAsync(It.IsAny<int>()), Times.Once);
                Assert.IsNotNull(photo, "Retrieved object cannot be null");
                Assert.AreEqual("TestPhoto", photo.Description, "Expected description should match actual");
                Assert.AreEqual("www.test.com.pl", photo.Url, "Expected description should match actual");
            });
        }

        [Test]
        public void VerifyDeletePhotoWasCalledTest()
        {
            //Arrange
            var testPhoto = new Photo() { Url = "www.test.pl", Description = "photo", MainPhoto = true };
            var dbContextMock = new Mock<Context>();

            //Act
            var petRepo = new PetRepository(dbContextMock.Object);
            petRepo.DeletePhoto(testPhoto);

            //Assert
            dbContextMock.Verify(x => x.Remove(testPhoto), Times.Once); ;
        }

        [Test]
        public void VerifyAddingLikeWasCalledTest()
        {
            //Arrange
            var testLike = new Likes() { Pet = new Pet() { Name = "Rex" }, PetId = 3, PetWhichLikedId = 1 };
            var dbContextMock = new Mock<Context>();
            var dbSetMock = new Mock<DbSet<Likes>>();
            dbSetMock.Setup(s => s.Add(It.Is<Likes>(like => like.PetWhichLikedId == testLike.PetWhichLikedId && like.PetId == testLike.PetId)));
            dbContextMock.Setup(s => s.Likes).Returns(dbSetMock.Object);

            //Act
            var petRepo = new PetRepository(dbContextMock.Object);
            petRepo.AddLike(testLike.PetWhichLikedId, testLike.PetId);

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Likes);
                dbSetMock.Verify(x => x.Add(It.Is<Likes>(like => like.PetWhichLikedId == testLike.PetWhichLikedId && like.PetId == testLike.PetId)), Times.Once);
            });
        }

        [Test]
        public void GetLikeByIdTest()
        {
            //Arrange
            List<int> expectedIds = new List<int>() { 2, 3, 4 };
            var testLikeId = 1;
            var dbContextMock = new Mock<Context>();
            var dbSetMock = GetMockDbSet<Likes>(likesTestData);
            dbContextMock.Setup(s => s.Likes).Returns(dbSetMock.Object);

            //Act
            var petRepo = new PetRepository(dbContextMock.Object);
            var actualLikesList = petRepo.GetLikes(testLikeId);

            //Assert
            Assert.Multiple(() =>
            {
                dbContextMock.VerifyGet(x => x.Likes);
                CollectionAssert.AreEquivalent(expectedIds, actualLikesList, "The number of expected Ids obtained by PetId should match actual and contain proper values");
            });
        }

        [Test, TestCaseSource(nameof(PagingDataProvider))]
        public void VerifyPagingMethodPageNumber3PageSize5(int pageNumber, int pageSize, int expectedItemsNumber, int expectedTotalPages)
        {
            //Arrange
            int numberOfPetsInTestData = pagingTestData.Count();
            var pagingMock = pagingTestData.AsQueryable().BuildMock().Object;

            //Act
            var page = PageList<Pet>.CreateAsync(pagingMock.AsQueryable(), pageNumber, pageSize).Result;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(page, "Created object cannot be null");
                Assert.IsInstanceOf(typeof(IList<Pet>), page, $"The created object should be of type {typeof(IEnumerable<Pet>)}");
                Assert.AreEqual(expectedItemsNumber, page.Count, $"Expected number of items for pageNumber '{pageNumber}' and PageSize '{pageSize}': {expectedItemsNumber}, but was {page.Count}");
                Assert.AreEqual(expectedTotalPages, page.TotalPages, $"Expected number of pages for pageNumber '{pageNumber}' and PageSize '{pageSize}': {expectedTotalPages}, but was {page.TotalPages}");
                Assert.AreEqual(13, pagingTestData.Count(), $"The number of used items should match expected");
            });
        }

        //pageNumber, pageSize, expectedItemsNumber, totalPages -> total items 13
        public static IEnumerable<int[]> PagingDataProvider()
        {
            yield return new int[] { 1, 4, 4, 4 };
            yield return new int[] { 4, 4, 1, 4 };
            yield return new int[] { 1, 8, 8, 2 };
            yield return new int[] { 2, 8, 5, 2 };
            yield return new int[] { 10, 4, 0, 4 };
            yield return new int[] { 5, 8, 0, 2 };
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