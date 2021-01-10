using Framework.API;
using Framework.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestDataProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            //Seed();
        }

        public static void Seed()
        {
            AuthorizationAPI authAPI = new AuthorizationAPI();
            var data = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SeedData.json");
            var pets = JsonConvert.DeserializeObject<List<Pet>>(data);
            foreach (var pet in pets)
            {
                authAPI.AddPet(pet);
            }
        }
    }
}
