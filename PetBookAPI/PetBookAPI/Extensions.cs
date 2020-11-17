using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBookAPI
{
    public static class Extensions
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int pageSize, int totalItems, int totalPages)
        {
            var header = new Pagination(currentPage, totalPages, pageSize, totalItems);
            var formatter = new JsonSerializerSettings();
            formatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(header, formatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
