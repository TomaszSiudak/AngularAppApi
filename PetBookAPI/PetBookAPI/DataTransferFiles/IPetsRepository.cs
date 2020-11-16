using PetBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.DataTransferFiles
{
    public interface IPetsRepository
    {
        Task<Pet> GetPet(int petId);
        Task<IEnumerable<Pet>> GetPets();
        void AddPet(Pet pet);
        void DeletePet(Pet pet);
        Task<bool> Save();
        Task<Photo> GetPhoto(int photoId);
        Task<Photo> GetMainPhoto(int petId);
        void DeletePhoto(Photo photo);
        void AddLike(int likerId, int petId);
        List<int> GetLikes(int petId);
    }
}
