﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PetBookAPI;
using PetBookAPI.Controllers;
using PetBookAPI.DataTransferFiles;
using PetBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Tests.Helpers;

namespace Tests
{
    [TestFixture]
    class ControllerTests
    {
        readonly List<Pet> petsData = new List<Pet>
        {
            new Pet() { Id = 1  ,Name = "Rex", Age = 5 },
            new Pet() { Id = 2  ,Name = "Salazar", Age = 3 },
            new Pet() { Id = 3  ,Name = "Bunia", Age = 1 },
            new Pet() { Id = 4  ,Name = "Mruczek", Age = 2 },
            new Pet() { Id = 5  ,Name = "Tomek", Age = 5 }
        };

        readonly List<Likes> likesTestData = new List<Likes>
        {
            new Likes() { Id = 1, Pet = new Pet(){ Name = "Rex" }, PetId = 1, PetWhichLikedId = 2 },
            new Likes() { Id = 3, Pet = new Pet(){ Name = "Rex" }, PetId = 1, PetWhichLikedId = 3 },
            new Likes() { Id = 5, Pet = new Pet(){ Name = "Rex" }, PetId = 1, PetWhichLikedId = 4 },
        };

        [Test]
        public void PetControllerGetPetTest()
        {
            //Arrange
            var pet = new Pet() { Id = 5, Name = "Salaz", Age = 3, Password = "test123" };
            var expectedPetDto = new PetDTO() { Id = 5, Name = "Salaz", Age = 3  };
            int expectedId =  2;
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            petRepoMock.Setup(s => s.GetPet(expectedId)).Returns(Task.FromResult(pet));
            mapper.Setup(s => s.Map<PetDTO>(pet)).Returns(expectedPetDto);

            //Act
            var petsController = new PetsController(petRepoMock.Object, mapper.Object);
            var controllerResult = petsController.GetPet(expectedId).Result;

            //Assert
            int? statusCode = ((OkObjectResult)controllerResult).StatusCode;
            PetDTO actualPetDto = ((OkObjectResult)controllerResult).Value as PetDTO;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(controllerResult);
                mapper.Verify(s => s.Map<PetDTO>(pet), Times.Once);
                Assert.AreEqual(200, statusCode, $"Expected status code {200}, but was {statusCode}");
                Assert.AreEqual(expectedPetDto.Name, actualPetDto.Name, $"Expected pet name of retrieved object {expectedPetDto.Name}, but was {actualPetDto.Name}");
            });
        }

        [Test]
        public void PetControllerUpdatePetCorrectActionTest()
        {
            //Arrange
            var pet = new Pet() { Id = 5, Name = "Salaz", Age = 3, Password = "test123" };
            var petDto = new EditPetDTO() { Name = "Rex", Age = 5 };
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            petRepoMock.Setup(s => s.GetPet(pet.Id)).Returns(Task.FromResult(pet));
            petRepoMock.Setup(s => s.Save()).Returns(Task.FromResult(true));
            mapper.Setup(s => s.Map(petDto, pet)).Returns(pet);
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>(){new Claim(ClaimTypes.NameIdentifier, pet.Id.ToString())});
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.Update(petDto, pet.Id).Result;

            //Assert
            int? statusCode = ((ObjectResult) controllerResult).StatusCode;
            EditPetDTO returnedObject = ((ObjectResult)controllerResult).Value as EditPetDTO;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<ObjectResult>(controllerResult);
                petRepoMock.Verify(s => s.Save(), Times.Once);
                Assert.AreEqual(201, statusCode, $"Expected status code {201}, but was {statusCode}");
                Assert.AreEqual(petDto.Name, returnedObject.Name, $"Expected name of returned object {petDto.Name}, but was {returnedObject.Name}");
                Assert.AreEqual(petDto.Age, returnedObject.Age, $"Expected age of returned object {petDto.Age}, but was {returnedObject.Age}");
            }); 
        }

        [Test]
        public void PetControllerUpdatePetUnathorizedEditionTest()
        {
            //Arrange
            var pet = new Pet() { Id = 5, Name = "Salaz", Age = 3, Password = "test123" };
            var petDto = new EditPetDTO() { Name = "Salaz", Age = 3 };
            int notAuthorizedPetId = 2;
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, pet.Id.ToString())});
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.Update(petDto, notAuthorizedPetId).Result;

            //Assert
            int? statusCode = ((UnauthorizedResult)controllerResult).StatusCode;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<IActionResult>(controllerResult);
                petRepoMock.Verify(s => s.GetPet(pet.Id), Times.Never);
                Assert.AreEqual(401, statusCode, $"Expected status code {401}, but was {statusCode}");
            });
        }

        [Test]
        public void PetControllerUpdatePetEditionErrorTest()
        {
            //Arrange
            string expectedErrorMessage = "Error: Server nie zapisał zmian";
            var pet = new Pet() { Id = 5, Name = "Salaz", Age = 3, Password = "test123" };
            var petDto = new EditPetDTO() { Name = "Rex", Age = 5 };
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            petRepoMock.Setup(s => s.GetPet(pet.Id)).Returns(Task.FromResult(pet));
            petRepoMock.Setup(s => s.Save()).Returns(Task.FromResult(false));
            mapper.Setup(s => s.Map(petDto, pet)).Returns(pet);
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, pet.Id.ToString()) });
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.Update(petDto, pet.Id).Result;

            //Assert
            int? statusCode = ((ObjectResult)controllerResult).StatusCode;
            string actualErrorMessage = ((ObjectResult)controllerResult).Value.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<IActionResult>(controllerResult);
                petRepoMock.Verify(s => s.Save(), Times.Once);
                Assert.AreEqual(400, statusCode, $"Expected status code {400}, but was {statusCode}");
                Assert.AreEqual(expectedErrorMessage, actualErrorMessage, $"Expected error  {expectedErrorMessage}, but was {actualErrorMessage}");
            });
        }

        [Test]
        public void PetControllerAddLikeCorrectActionTest()
        {
            //Arrange
            var petId = 1;
            var PetWhichLikedId = 5; //not existing in Likes tests data
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            petRepoMock.Setup(s => s.GetPet(petId)).Returns(Task.FromResult(new Pet() { Name = "Rex", Likes = likesTestData }));
            petRepoMock.Setup(s => s.AddLike(PetWhichLikedId, petId));
            petRepoMock.Setup(s => s.Save()).Returns(Task.FromResult(true));
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, PetWhichLikedId.ToString()) });
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.AddLike(petId, PetWhichLikedId).Result;

            //Assert
            int? statusCode = ((OkResult)controllerResult).StatusCode;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkResult>(controllerResult);
                petRepoMock.Verify(s => s.AddLike(PetWhichLikedId, petId), Times.Once);
                Assert.AreEqual(200, statusCode, $"Expected status code {200}, but was {statusCode}");
            });
        }

        [Test]
        public void PetControllerAddLikeWhichHasAlreadyBeenAddedTest()
        {
            //Arrange
            string expectedErrorMessage = "Użytkownik został już polubiony";
            var petId = 1;
            var PetWhichLikedId = 3; //existing in Likes tests data
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            petRepoMock.Setup(s => s.GetPet(petId)).Returns(Task.FromResult(new Pet() { Name = "Rex", Likes = likesTestData }));
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, PetWhichLikedId.ToString()) });
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.AddLike(petId, PetWhichLikedId).Result;

            //Assert
            int? statusCode = ((ObjectResult)controllerResult).StatusCode;
            string actualErrorMessage = ((ObjectResult)controllerResult).Value.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<BadRequestObjectResult>(controllerResult);
                petRepoMock.Verify(s => s.AddLike(PetWhichLikedId, petId), Times.Never);
                Assert.AreEqual(400, statusCode, $"Expected status code {400}, but was {statusCode}");
                Assert.AreEqual(expectedErrorMessage, actualErrorMessage, $"Expected error  {expectedErrorMessage}, but was {actualErrorMessage}");
            });
        }

        [Test]
        public void PetControllerAddLikeSaveErrorTest()
        {
            //Arrange
            string expectedErrorMessage = "Error: Server nie zapisał zmian";
            var petId = 1;
            var PetWhichLikedId = 6; //not existing in Likes tests data
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            petRepoMock.Setup(s => s.GetPet(petId)).Returns(Task.FromResult(new Pet() { Name = "Rex", Likes = likesTestData }));
            petRepoMock.Setup(s => s.AddLike(PetWhichLikedId, petId));
            petRepoMock.Setup(s => s.Save()).Returns(Task.FromResult(false));
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, PetWhichLikedId.ToString()) });
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.AddLike(petId, PetWhichLikedId).Result;

            //Assert
            int? statusCode = ((ObjectResult)controllerResult).StatusCode;
            string actualErrorMessage = ((ObjectResult)controllerResult).Value.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<BadRequestObjectResult>(controllerResult);
                petRepoMock.Verify(s => s.AddLike(PetWhichLikedId, petId), Times.Once);
                Assert.AreEqual(400, statusCode, $"Expected status code {400}, but was {statusCode}");
                Assert.AreEqual(expectedErrorMessage, actualErrorMessage, $"Expected error  {expectedErrorMessage}, but was {actualErrorMessage}");
            });
        }

        [Test]
        public void PetControllerAddLikeUnauthorizedUserTest()
        {
            //Arrange
            var petId = 2;
            var PetWhichLikedIdNotLoggedIn = 6; //not existing in Likes tests data
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, petId.ToString()) });
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.AddLike(petId, PetWhichLikedIdNotLoggedIn).Result;

            //Assert
            int? statusCode = ((UnauthorizedResult)controllerResult).StatusCode;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<UnauthorizedResult>(controllerResult);
                petRepoMock.Verify(s => s.GetPet(petId), Times.Never);
                Assert.AreEqual(401, statusCode, $"Expected status code {401}, but was {statusCode}");
            });
        }

        [Test]
        public void PetControllerGetLikesCorrectActionTest()
        {
            //Arrange
            var PetsWhichLikedIds = likesTestData.Select(like => like.PetWhichLikedId).ToList();
            var expectedPetsList = petsData.Where(pet => PetsWhichLikedIds.Contains(pet.Id)).Select(pet => new PetDTO() { Name = pet.Name, Age = pet.Age });
            var petId = 1;
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            petRepoMock.Setup(s => s.GetLikes(petId)).Returns(PetsWhichLikedIds);
            petRepoMock.SetupSequence(s => s.GetPet(It.IsAny<int>()))
                .Returns(Task.FromResult(petsData.FirstOrDefault(pet => pet.Id == PetsWhichLikedIds[0])))
                .Returns(Task.FromResult(petsData.FirstOrDefault(pet => pet.Id == PetsWhichLikedIds[1])))
                .Returns(Task.FromResult(petsData.FirstOrDefault(pet => pet.Id == PetsWhichLikedIds[2])));
            mapper.Setup(s => s.Map<IEnumerable<PetDTO>>(It.IsAny<IEnumerable<Pet>>())).Returns(expectedPetsList);

            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, petId.ToString()) });
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.GetLikes(petId).Result;

            //Assert
            int? statusCode = ((OkObjectResult)controllerResult).StatusCode;
            var actualPetDto = ((OkObjectResult)controllerResult).Value as IEnumerable<PetDTO>;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(controllerResult);
                petRepoMock.Verify(s => s.GetLikes(petId), Times.Once);
                petRepoMock.Verify(s => s.GetPet(It.IsAny<int>()), Times.Exactly(PetsWhichLikedIds.Count));
                Assert.AreEqual(200, statusCode, $"Expected status code {200}, but was {statusCode}");
                CollectionAssert.AreEquivalent(expectedPetsList.Select(p => p.Name), actualPetDto.Select(p => p.Name));
            });
        }

        [Test]
        public void PetControllerGetLikesUnauthorizedUserTest()
        {
            //Arrange
            var petId = 1;
            var PetWhichLikedIdNotLoggedIn = 6;
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, petId.ToString()) });
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petsController = new PetsController(petRepoMock.Object, mapper.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = claimsPrincipal
                    }
                }
            };

            //Act
            var controllerResult = petsController.GetLikes(PetWhichLikedIdNotLoggedIn).Result;

            //Assert
            int? statusCode = ((UnauthorizedResult)controllerResult).StatusCode;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<UnauthorizedResult>(controllerResult);
                petRepoMock.Verify(s => s.GetLikes(It.IsAny<int>()), Times.Never);
                Assert.AreEqual(401, statusCode, $"Expected status code {401}, but was {statusCode}");
            });
        }

        [Test]
        public void AuthControllerVerifyPetRegistrationTest()
        {
            //Arrange
            string expectedRouteName = "GetPet";
            string expectedControllerName = "Pets";
            var newPet = new Pet() { Id = 5, Name = "Mruczek", Age = 3, Password = "test123", Type = "cat", City = "Katowice" };
            int expectedIdUsedInController = newPet.Id;
            var petDto = new PetDTO() { Name = newPet.Name, Age = newPet.Age, City = newPet.City };
            var authRepoMock = new Mock<IAuthorizationRepository>();
            var mapper = new Mock<IMapper>();
            var config = new Mock<IConfiguration>();
            authRepoMock.Setup(s => s.Exists(newPet.Name)).Returns(false);
            authRepoMock.Setup(s => s.Register(newPet)).Returns(Task.FromResult(newPet));
            mapper.Setup(s => s.Map<PetDTO>(newPet)).Returns(petDto);
            var authController = new AuthorizationController(authRepoMock.Object, config.Object, mapper.Object);

            //Act
            var controllerResult = authController.Register(newPet).Result;
            var routeResult = (CreatedAtRouteResult)controllerResult;
            
            //Assert
            int? statusCode = routeResult.StatusCode;
            string routeName = routeResult.RouteName;
            var routeValues = routeResult.RouteValues;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<ObjectResult>(controllerResult);
                authRepoMock.Verify(s => s.Register(It.IsAny<Pet>()), Times.Once);
                Assert.AreEqual(201, statusCode, $"Expected status code {201}, but was {statusCode}");
                Assert.AreEqual(expectedRouteName, routeName, $"Expected route name {expectedRouteName}, but was {routeName}");
                Assert.AreEqual(expectedControllerName, routeValues["controller"], $"Expected controller name {expectedControllerName}, but was {routeValues["controller"]}");
                Assert.AreEqual(expectedIdUsedInController, routeValues["petId"], $"Expected petId used in controller {expectedIdUsedInController}, but was {routeValues["petId"]}");
            });
        }

        [Test]
        public void AuthControllerVerifyRegistrationPetAlreadyExistsTest()
        {
            //Arrange
            string expectedErrorMessage = "Użytkownik o tej nazwie już istnieje. Wybierz inną.";
            var newPet = new Pet() { Id = 5, Name = "Mruczek", Age = 3, Password = "test123", Type = "cat", City = "Katowice" };
            var authRepoMock = new Mock<IAuthorizationRepository>();
            var mapper = new Mock<IMapper>();
            var config = new Mock<IConfiguration>();
            authRepoMock.Setup(s => s.Exists(newPet.Name)).Returns(true);
            authRepoMock.Setup(s => s.Register(newPet)).Returns(Task.FromResult(newPet));
            var authController = new AuthorizationController(authRepoMock.Object, config.Object, mapper.Object);

            //Act
            var controllerResult = authController.Register(newPet).Result;
            var objectResult = (ObjectResult)controllerResult;

            //Assert
            int? statusCode = objectResult.StatusCode;
            string actualErrorMessage = objectResult.Value.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<BadRequestObjectResult>(controllerResult);
                authRepoMock.Verify(s => s.Register(It.IsAny<Pet>()), Times.Never);
                Assert.AreEqual(400, statusCode, $"Expected status code {400}, but was {statusCode}");
                Assert.AreEqual(expectedErrorMessage, actualErrorMessage, $"Expected error  {expectedErrorMessage}, but was {actualErrorMessage}");
            });
        }

        [Test]
        public void AuthControllerVerifyPetCorrectLoginTest()
        {
            //Arrange
            var pet = new Pet() { Id = 5, Name = "Mruczek", Age = 3, Password = "test123", Type = "cat", City = "Katowice" };
            var authRepoMock = new Mock<IAuthorizationRepository>();
            var mapper = new Mock<IMapper>();
            var config = new Mock<IConfiguration>();
            var oneSectionMock = new Mock<IConfigurationSection>();
            oneSectionMock.Setup(s => s.Value).Returns("asdfghjklqweryuiop");
            authRepoMock.Setup(s => s.Login(pet.Name, pet.Password)).Returns(Task.FromResult(pet));
            config.Setup(s => s.GetSection(It.IsAny<string>())).Returns(oneSectionMock.Object);
            var authController = new AuthorizationController(authRepoMock.Object, config.Object, mapper.Object);

            //Act
            var controllerResult = authController.Login(pet).Result;
            var okResult = (OkObjectResult)controllerResult;

            //Assert
            int? statusCode = okResult.StatusCode;
            var obj = okResult.Value;
            var token = obj.GetType().GetProperty("token").GetValue(obj).ToString(); 
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<OkObjectResult>(controllerResult);
                authRepoMock.Verify(s => s.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                Assert.AreEqual(200, statusCode, $"Expected status code {200}, but was {statusCode}");
                Assert.IsTrue(!string.IsNullOrEmpty(token), "Created token cannot be empty");
            });
        }

        [Test]
        public void AuthControllerVerifyPetNOTCorrectLoginTest()
        {
            //Arrange
            string expectedErrorMessage = "Nieprawidłowy login lub hasło";
            var pet = new Pet() { Id = 5, Name = "Mruczek", Password = "test" };
            int expectedIdUsedInController = pet.Id;
            var authRepoMock = new Mock<IAuthorizationRepository>();
            var mapper = new Mock<IMapper>();
            var config = new Mock<IConfiguration>();
            authRepoMock.Setup(s => s.Login(pet.Name, pet.Password)).Returns(Task.FromResult((Pet)null));
            var authController = new AuthorizationController(authRepoMock.Object, config.Object, mapper.Object);

            //Act
            var controllerResult = authController.Login(pet).Result;
            var result = (UnauthorizedObjectResult)controllerResult;

            //Assert
            int? statusCode = result.StatusCode;
            string actualErrorMessage = result.Value.ToString();
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<UnauthorizedObjectResult>(controllerResult);
                authRepoMock.Verify(s => s.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                Assert.AreEqual(401, statusCode, $"Expected status code {401}, but was {statusCode}");
                Assert.AreEqual(expectedErrorMessage, actualErrorMessage, $"Expected error  {expectedErrorMessage}, but was {actualErrorMessage}");
            });
        }
    }
}
