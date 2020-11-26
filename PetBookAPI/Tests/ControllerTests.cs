using AutoMapper;
using AutoMapper.Configuration;
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


namespace Tests
{
    [TestFixture]
    class ControllerTests
    {

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
            var petRepo = new PetsController(petRepoMock.Object, mapper.Object);
            var controllerResult = petRepo.GetPet(expectedId).Result;

            //Assert
            int? statusCode = ((OkObjectResult)controllerResult).StatusCode;
            PetDTO actualPetDto = ((OkObjectResult)controllerResult).Value as PetDTO;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<IActionResult>(controllerResult);
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
            var petDto = new EditPetDTO() { Name = "Salaz", Age = 3 };
            var petRepoMock = new Mock<IPetsRepository>();
            var mapper = new Mock<IMapper>();
            petRepoMock.Setup(s => s.GetPet(pet.Id)).Returns(Task.FromResult(pet));
            petRepoMock.Setup(s => s.Save()).Returns(Task.FromResult(true));
            mapper.Setup(s => s.Map(petDto, pet)).Returns(pet);
            var claimsPrincipal = new ClaimsPrincipal();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>(){new Claim(ClaimTypes.NameIdentifier, pet.Id.ToString())});
            claimsPrincipal.AddIdentity(claimsIdentity);

            var petRepo = new PetsController(petRepoMock.Object, mapper.Object)
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
            var controllerResult = petRepo.Update(petDto, pet.Id).Result;

            //Assert
            int? statusCode = ((NoContentResult) controllerResult).StatusCode;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<IActionResult>(controllerResult);
                petRepoMock.Verify(s => s.Save(), Times.Once);
                Assert.AreEqual(204, statusCode, $"Expected status code {204}, but was {statusCode}");
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

            var petRepo = new PetsController(petRepoMock.Object, mapper.Object)
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
            var controllerResult = petRepo.Update(petDto, notAuthorizedPetId).Result;

            //Assert
            int? statusCode = ((UnauthorizedResult)controllerResult).StatusCode;
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf<IActionResult>(controllerResult);
                Assert.AreEqual(401, statusCode, $"Expected status code {401}, but was {statusCode}");
            });
        }
    }
}
