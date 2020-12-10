using Framework.API;
using Framework.Constants;
using Framework.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Extensions
{
    public static class RESTExtensions
    {
        public static void AddRESTHeaders(this RestRequest request, bool withAuthorization = true, DataFormat requestFormat = DataFormat.Json, string contentType = "application/json")
        {
            request.RequestFormat = requestFormat;
            request.AddHeader("Content-Type", contentType);
            if (withAuthorization) request.AddHeader("Authorization", string.Format("Bearer {0}", GetToken())); ;
        }

        public static JObject GetJObjectFromResponse(this IRestResponse response)
        {
            return JsonConvert.DeserializeObject<JObject>(response.Content);
        }

        public static T DeserializeResponse<T>(this IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }


        private static string GetToken() => new AuthorizationAPI().Login(Variables.DefaultPet).GetJObjectFromResponse().Value<string>("token");
    }
}
