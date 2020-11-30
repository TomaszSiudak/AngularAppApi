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
        public static void AddRESTHeaders(this RestRequest request, DataFormat requestFormat = DataFormat.Json, string contentType = "application/json")
        {
            request.RequestFormat = requestFormat;
            request.AddHeader("Content-Type", contentType);
        }

        public static JObject GetJObjectFromResponse(this IRestResponse response)
        {
            return JsonConvert.DeserializeObject<JObject>(response.Content);
        }
    }
}
