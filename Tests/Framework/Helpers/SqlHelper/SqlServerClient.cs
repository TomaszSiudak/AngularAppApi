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
        private static string ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=PetBook;Trusted_Connection=true;";


        protected static List<Dictionary<string, string>> ExecuteCommand(string queryString)
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
