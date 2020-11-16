using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.Model
{
    public class Likes
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
        public int PetWhichLikedId { get; set; }
    }

}
