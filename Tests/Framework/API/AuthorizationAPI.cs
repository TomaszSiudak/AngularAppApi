using Framework.Base;
using Framework.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Extensions;

namespace Framework.API
{
    public class AuthorizationAPI : BaseAPI
    {
        private static string controllerName = "authorization";


        public IRestResponse Login(Pet pet)
        {
            restRequest = new RestRequest($"{controllerName}/login", Method.POST);
            restRequest.AddJsonBody(pet);
            var response = restClient.Execute(restRequest);
            return response;
        }

        public IRestResponse Register(Pet pet)
        {
            restRequest = new RestRequest($"{controllerName}/registration", Method.POST);
            restRequest.AddRESTHeaders();
            restRequest.AddJsonBody(pet);
            var response = restClient.Execute(restRequest);
            return response;
        }
    }
}
