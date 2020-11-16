using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.Model
{
    public class CloudPhotoDto
    {
        public IFormFile File { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string PublicId { get; set; }

    }
}
