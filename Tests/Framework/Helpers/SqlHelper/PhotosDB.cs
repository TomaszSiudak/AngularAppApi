using Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helpers.SqlHelper
{
    public class PhotosDB : SqlServerClient
    {
        public List<Photo> GetPetPhotos(string petId)
        {
            List<Photo> photos = new List<Photo>();
            string query =
                $" SELECT * FROM dbo.Photos " +
                $" WHERE PetId = {petId}";

            var rows = ExecuteCommand(query);

            foreach (var row in rows)
            {
                Photo photo = new Photo()
                {
                    Id = int.Parse(row["Id"]),
                    Url = row["Url"],
                    Description = row["Description"],
                    MainPhoto = Convert.ToBoolean(row["MainPhoto"])
                };
                photos.Add(photo);
            }
            return photos;
        }
    }
}
