using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.Model
{
    public class EditPetDTO
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
    }
}
