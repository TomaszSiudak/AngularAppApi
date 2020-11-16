using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.Model
{
    public class Pet
    { 
        public int Id { get; set; }
        [Required (ErrorMessage = "Nazwa użytkownika jest wymagana i nie może być pusta")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane i musi zawierać co najmniej 3 znaki")]
        [MinLength(3)]
        public string Password { get; set; }
        public int Age { get; set; }
        public string Gender{ get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public List<Photo> Photos { get; set; }
        public List<Likes> Likes { get; set; }

    }
}
