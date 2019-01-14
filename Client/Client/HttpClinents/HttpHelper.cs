using System;
using System.Net.Http;

namespace Client.HttpClinents
{
    public abstract class HttpHelper
    {
        protected readonly HttpClient httpClient;
        protected static string CURRENTTOKEN;

        private string _baseURL;
        protected string BaseURL
        {
            get => _baseURL;
            set
            {
                _baseURL = value;
                httpClient.BaseAddress = new Uri(value);
            }
        }

        protected HttpHelper(string baseURL)
        {
            httpClient = new HttpClient();
            BaseURL = baseURL;
            httpClient.DefaultRequestHeaders.Add("Token", CURRENTTOKEN);
        }

        ~HttpHelper()
        {
            httpClient.Dispose();
        }

        public static void DeleteToken()
        {
            CURRENTTOKEN = null;
        }
    }
}
