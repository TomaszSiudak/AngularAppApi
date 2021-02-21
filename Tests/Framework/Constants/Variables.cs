using Framework.Models;

namespace Framework.Constants
{
    public class Variables
    {
        public static Pet DefaultPet { get { return new Pet() { Name = "Tom", Password = "test" }; } } 
    }
}
