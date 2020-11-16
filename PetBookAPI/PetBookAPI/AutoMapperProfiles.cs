using AutoMapper;
using PetBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Pet, PetDTO>().ForMember(destinationMember => destinationMember.Photo, options => options.MapFrom(pet => pet.Photos.Where(p => p.MainPhoto == true).Select(p => p.Url).FirstOrDefault()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<EditPetDTO, Pet>();
            CreateMap<CloudPhotoDto, Photo>();
            CreateMap<Likes, LikesDTO>();
        }
    }
}
