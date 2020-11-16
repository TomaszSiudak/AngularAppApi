using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetBookAPI.DataTransferFiles;
using PetBookAPI.Model;

namespace PetBookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        public IPetsRepository petsRepository;
        public IMapper mapper;

        public PetsController(IPetsRepository petsRepository, IMapper mapper)
        {
            this.petsRepository = petsRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPets()
        {
            //List<PetDTO> petsDTO = new List<PetDTO>();
            var pets = await petsRepository.GetPets();

            var petsDTO = mapper.Map<IEnumerable<PetDTO>>(pets);


            return Ok(petsDTO);
        }

        [HttpGet("{petId}", Name ="GetPet")]
        public async Task<IActionResult> GetPet(int petId)
        {
            var pet = await petsRepository.GetPet(petId);

            var petDTO = mapper.Map<PetDTO>(pet);

            return Ok(petDTO);
        }

        [HttpPut("{petId}")]
        public async Task<IActionResult> Update(EditPetDTO editPetDTO , int petId)
        {
            if (petId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var pet = await petsRepository.GetPet(petId);

            mapper.Map(editPetDTO, pet);

            if (await petsRepository.Save())
            {
                return NoContent();
            }
            throw new Exception("Server nie zapisał zmian");

        }

        [HttpPost("{petId}/likes/{likerId}")]
        public async Task<IActionResult> AddLike(int petId, int likerId)
        {
            if (likerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var pet = await petsRepository.GetPet(petId);

            if (pet.Likes != null && pet.Likes.Select(like => like.PetWhichLikedId).Contains(likerId))
            {
                return BadRequest("Użytkownik został już polubiony");
            }
            else 
            {
                petsRepository.AddLike(likerId, petId);
            }

            if (await petsRepository.Save())
            {
                return Ok();
            }
            return BadRequest("Server nie zapisał zmian");

        }

        [HttpGet("{petId}/likes")]
        public async Task<IActionResult> GetLikes(int petId)
        {
            List<Pet> pets = new List<Pet>();
            if (petId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var petLikes = petsRepository.GetLikes(petId);

            foreach (var likeId in petLikes)
            {
                var pet = await petsRepository.GetPet(likeId);
                pets.Add(pet);
            }

            var petsDTO = mapper.Map<IEnumerable<PetDTO>>(pets);

            return Ok(petsDTO);
        }
    }
}