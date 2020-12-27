
using RestSharp;
using System.Configuration;


namespace Framework.Base
{
    public abstract class BaseAPI
    {
        public static string Environment = ConfigurationManager.AppSettings["apiBaseURL"];
        protected RestClient restClient;
        protected RestRequest restRequest;

        protected BaseAPI()
        {
            this.restClient = new RestClient($"{Environment}");
        }

    }
}
