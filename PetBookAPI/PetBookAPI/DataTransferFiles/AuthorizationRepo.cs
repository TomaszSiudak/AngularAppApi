using PetBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.DataTransferFiles
{
    public class AuthorizationRepo : IAuthorizationRepository
    {
        public Context Context { get; }

        public AuthorizationRepo(Context context)
        {
            Context = context;
        }

        public bool Exists(string name)
        {
           return Context.Pets.Any(p => p.Name.Equals(name));
        }

        public async Task<Pet> Login(string name, string password)
        {
            var pet = Context.Pets.Where(p=> p.Name.Equals(name)).SingleOrDefault();
            if (pet != null)
            {
                if (pet.Password.Equals(password))
                {
                    return pet;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }

        public async Task<Pet> Register(Pet pet)
        {
            await Context.Pets.AddAsync(pet);
            await Context.SaveChangesAsync();
            return pet;
        }
    }
}
