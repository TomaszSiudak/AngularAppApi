using Framework.Base;
using Framework.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Extensions;
using Framework.Helpers.SqlHelper;

namespace Framework.API
{
    public class PetsAPI : BaseAPI
    {
        private static string controllerName = "pets";

        public IRestResponse GetPetById(int id)
        {
            restRequest = new RestRequest($"{controllerName}/{id}");
            restRequest.AddRESTHeaders();
            var response = restClient.Execute(restRequest);
            return response;
        }

        public IRestResponse GetPetsBySearchCriteria(PetQueryParameters parameters)
        {
            restRequest = new RestRequest($"{controllerName}", Method.GET);
            restRequest.AddRESTHeaders();
            AddQueryParameters(restRequest, parameters);
            var response = restClient.Execute(restRequest);
            return response;
        }

        public IRestResponse UpdatePet(Pet pet, int id, Pet petToAuthorize = null)
        {
            restRequest = new RestRequest($"{controllerName}/{id}", Method.PUT);
            restRequest.AddRESTHeaders(petToAuthorize: petToAuthorize);
            restRequest.AddJsonBody(pet);
            return restClient.Execute(restRequest);
        }

        private void AddQueryParameters(RestRequest restRequest, PetQueryParameters parameters)
        {
            foreach (var field in parameters.GetType().GetFields())
            {
                restRequest.AddParameter(field.Name, field.GetValue(parameters));
            }
        }

    }



    public class PetQueryParameters
    {
        public int currentPage;
        public int pageSize;
        public string gender;
        public string type;

        public PetQueryParameters()
        {
        }

        public PetQueryParameters(string gender, string type, int currentPage = 1, int pageSize = 8)
        {
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            this.gender = gender;
            this.type = type;
        }
    }
}
