using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Constants
{
    public class Variables
    {
        public static Pet DefaultPet { get { return new Pet() { Name = "Tom", Password = "test" }; } } 
    }
}
