using Framework.Constants;
using Framework.Models;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Helpers.SqlHelper
{
    public class PetsDB : SqlServerClient
    {

        public bool IsPetInDb(string name)
        {
            string query = $"SELECT * FROM dbo.Pets WHERE Name = '{name}'";

            return ExecuteCommand(query).Count == 1 ? true : false;
        }

        public Pet GetPetById(int id)
        {
            string query = $"SELECT TOP 1 * FROM dbo.Pets WHERE Id = '{id}'";

            return GetPetFromDb(query);
        }

        public Pet GetPetByName(string name)
        {
            string query = $"SELECT TOP 1 * FROM dbo.Pets WHERE Name = '{name}'";

            return GetPetFromDb(query);
        }

        public Pet GetRandomPet()
        {
            string query = $"SELECT TOP 1 * FROM dbo.Pets ORDER BY NEWID()";

            return GetPetFromDb(query);
        }

        public List<Pet> GetPets()
        {
            string query = $"SELECT * FROM dbo.Pets ";

            return GetPetsFromDb(query);
        }

        public List<Pet> GetPetsByCriterium(string key, string value)
        {
            string query =
                $" SELECT * FROM dbo.Pets " +
                $" WHERE {key} = '{value}'";

            return GetPetsFromDb(query);
        }

        private static Pet GetPetFromDb(string query)
        {
            var row = ExecuteCommand(query).FirstOrDefault();

            if (row != null)
            {
                Pet pet = new Pet()
                {
                    Id = int.Parse(row[PetsColumns.Id]),
                    Password = row[PetsColumns.Password],
                    Name = row[PetsColumns.Name],
                    Age = int.Parse(row[PetsColumns.Age]),
                    Type = row[PetsColumns.Type],
                    Description = row[PetsColumns.Description],
                    City = row[PetsColumns.City],
                    Gender = row[PetsColumns.Gender]
                };
                return pet;
            }
            return null;
        }

        private static List<Pet> GetPetsFromDb(string query)
        {
            List<Pet> pets = new List<Pet>();
            var rows = ExecuteCommand(query);

            foreach (var row in rows)
            {
                Pet pet = new Pet()
                {
                    Id = int.Parse(row[PetsColumns.Id]),
                    Name = row[PetsColumns.Name],
                    Age = int.Parse(row[PetsColumns.Age]),
                    Type = row[PetsColumns.Type],
                    Description = row[PetsColumns.Description],
                    City = row[PetsColumns.City],
                    Gender = row[PetsColumns.Gender]
                };
                pets.Add(pet);
            }
            return pets;
        }
    }
}
