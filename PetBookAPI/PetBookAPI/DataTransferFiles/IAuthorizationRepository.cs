using PetBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.DataTransferFiles
{
    public interface IAuthorizationRepository
    {
        Task<Pet> Register(Pet pet);
        Task<Pet> Login(string name, string password);
        bool Exists(string name);
    }
}
