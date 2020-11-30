using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Base
{
    public abstract class BaseAPI
    {
        public static string Environment = ConfigurationManager.AppSettings["environment"];
        protected RestClient restClient;
        protected RestRequest restRequest;

        protected BaseAPI()
        {
            this.restClient = new RestClient($"{Environment}");
        }
    }
}
