using Microsoft.EntityFrameworkCore;
using PetBookAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI.DataTransferFiles
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }

    }
}
