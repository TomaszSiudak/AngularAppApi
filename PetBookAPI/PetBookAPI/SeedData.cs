using Newtonsoft.Json;
using PetBookAPI.DataTransferFiles;
using PetBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI
{
    public class SeedData
    {
        private readonly Context context;
        public SeedData(Context context)
        {
            this.context = context;
        }

        public void Seed()
        {
            var data = System.IO.File.ReadAllText("SeedData.json");
            var pets = JsonConvert.DeserializeObject<List<Pet>>(data);
            foreach (var pet in pets)
            { 
                context.Pets.Add(pet);
            }

            context.SaveChanges();
        }
    }
}
