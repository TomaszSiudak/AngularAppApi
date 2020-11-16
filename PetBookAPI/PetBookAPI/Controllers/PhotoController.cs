using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetBookAPI.DataTransferFiles;
using PetBookAPI.Model;

namespace PetBookAPI.Controllers
{
    [Authorize]
    [Route("api/pets/{petId}/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        public IPetsRepository petsRepository;
        public IMapper mapper;
        private Cloudinary cloud;

        public PhotoController(IPetsRepository petsRepository, IMapper mapper)
        {
            this.petsRepository = petsRepository;
            this.mapper = mapper;
            Account account = new Account(Cloud.CloudName, Cloud.APIKey, Cloud.APISecret);
            cloud = new Cloudinary(account);
        }

        [HttpGet("{photoId}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int photoId)
        {
            var photo = await petsRepository.GetPhoto(photoId);

            var photoDTO = mapper.Map<PhotoDTO>(photo);

            return Ok(photoDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(int petId, [FromForm] CloudPhotoDto cloudPhotoDto)
        {
            if (petId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var pet = await petsRepository.GetPet(petId);
            var file = cloudPhotoDto.File;
            var imageUploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var imageUploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    imageUploadResult = cloud.Upload(imageUploadParams);
                }
            }

            cloudPhotoDto.Url = imageUploadResult.Url.ToString();
            cloudPhotoDto.PublicId = imageUploadResult.PublicId;
            var photo = mapper.Map<Photo>(cloudPhotoDto);

            if (!pet.Photos.Any(p => p.MainPhoto))
                photo.MainPhoto = true;

            pet.Photos.Add(photo);

            if (await petsRepository.Save())
            {
                var photoDTO = mapper.Map<PhotoDTO>(photo);
                return CreatedAtRoute("GetPhoto", new { photoId = photo.Id }, photoDTO);
            }

            return BadRequest("Nie udało się załadować zdjęcia");
        }

        [HttpPost("{photoId}/setMainPhoto")]
        public async Task<IActionResult> SetMainPhoto(int petId, int photoId)
        {
            if (petId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var pet = await petsRepository.GetPet(petId);

            if (!pet.Photos.Any(p => p.Id == photoId))
                return BadRequest("The photo does not belong to the user");

            var photo = await petsRepository.GetPhoto(photoId);
            var currentMainPhoto = await petsRepository.GetMainPhoto(petId);
            currentMainPhoto.MainPhoto = false;
            photo.MainPhoto = true;

            if (await petsRepository.Save())
                return NoContent();

            return BadRequest("Error with changing photo");
        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> Delete(int petId, int photoId)
        {
            if (petId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await petsRepository.GetPet(petId);

            if (!user.Photos.Any(p => p.Id == photoId))
                return BadRequest("The photo does not belong to the user");

            var photo = await petsRepository.GetPhoto(photoId);

            if (photo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photo.PublicId);
                var result = cloud.Destroy(deleteParams);

                if (result.Result.Equals("ok"))
                {
                    petsRepository.DeletePhoto(photo);
                }
            } else
            {
                petsRepository.DeletePhoto(photo);
            }

            if (await petsRepository.Save())
                return Ok();

            return BadRequest("Error when deleting photo");
        }
    }
}