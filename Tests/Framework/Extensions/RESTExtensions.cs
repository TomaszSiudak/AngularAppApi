using Framework.Helpers;
using Framework.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Framework.Extensions
{
    public static class RESTExtensions
    {
        public static void AddRESTHeaders(this RestRequest request, bool withAuthorization = true, Pet petToAuthorize = null, DataFormat requestFormat = DataFormat.Json, string contentType = "application/json")
        {
            request.RequestFormat = requestFormat;
            request.AddHeader("Content-Type", contentType);
            if (withAuthorization) request.AddHeader("Authorization", string.Format("Bearer {0}", AuthenticationHelper.GetToken(petToAuthorize)));
        }

        public static JObject GetJObjectFromResponse(this IRestResponse response)
        {
            return JsonConvert.DeserializeObject<JObject>(response.Content);
        }

        public static T DeserializeResponse<T>(this IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

    }
}
