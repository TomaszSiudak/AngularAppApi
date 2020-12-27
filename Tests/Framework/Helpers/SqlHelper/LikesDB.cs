using System.Collections.Generic;

namespace Framework.Helpers.SqlHelper
{
    public class LikesDB : SqlServerClient
    {
        public List<int> GetLikesOfPetById(int petId)
        {
            List<int> petLikesIds = new List<int>();
            string query = $"SELECT [PetWhichLikedId] FROM dbo.Likes WHERE PetId = '{petId}'";

            var rows = ExecuteCommand(query);

            foreach (var row in rows)
            {
                petLikesIds.Add(int.Parse(row["PetWhichLikedId"]));
            }
            return petLikesIds;
        }
    }
}
