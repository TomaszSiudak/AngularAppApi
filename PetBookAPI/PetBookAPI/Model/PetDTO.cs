using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.Model
{
    public class PetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Photo { get; set; }
        public List<PhotoDTO> Photos { get; set; }
        public List<LikesDTO> Likes { get; set; }
    }
}
