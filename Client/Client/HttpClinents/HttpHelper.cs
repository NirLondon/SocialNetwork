using System;
using System.Net.Http;

namespace Client.HttpClinents
{
    public abstract class HttpHelper
    {
        protected readonly HttpClient httpClient;
        private static string _currentToken = string.Empty;
        protected void SetCurrentToken(string value)
        {
            lock (_currentToken)
            {
                _currentToken = value;
                httpClient.DefaultRequestHeaders.Remove("Token");
                httpClient.DefaultRequestHeaders.Add("Token", value);
            }
        }
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
            SetCurrentToken(_currentToken);
        }

        ~HttpHelper()
        {
            httpClient.Dispose();
        }
    }
}
