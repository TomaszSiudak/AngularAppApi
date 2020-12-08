using Framework.Constants;
using Framework.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helpers
{
    public class SqlServerClient
    {
        private static string ConnectionString = "Data Source=TOM\\SQLEXPRESS01;Initial Catalog=PetBook;Trusted_Connection=true;";

        public static bool IsPetInDb(string name)
        {
            string query = $"SELECT TOP 1 * FROM dbo.Pets WHERE Name = '{name}'";

            return ExecuteCommand(query).Count == 1 ? true : false;
        }

        public static Pet GetPetByName(string name)
        {
            string query = $"SELECT TOP 1 * FROM dbo.Pets WHERE Name = '{name}'";

            var row = ExecuteCommand(query).FirstOrDefault();

            if (row != null)
            {
                Pet pet = new Pet()
                {
                    Id = int.Parse(row[PetsColumns.Id]),
                    Name = row[PetsColumns.Name],
                    Age = int.Parse(row[PetsColumns.Age]),
                    Type = row[PetsColumns.Type],
                    Description = row[PetsColumns.Description],
                    City = row[PetsColumns.City],
                    Gender = row[PetsColumns.Gender]
                };
                return pet;
            }
            return null;
        }



        private static List<Dictionary<string, string>> ExecuteCommand(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        string key = dataTable.Columns[j].ToString();
                        string value = dataTable.Rows[i][j].ToString();

                        dict.Add(key, value);
                    }
                    rows.Add(dict);
                }

                return rows;
            }
        }
    }
}
