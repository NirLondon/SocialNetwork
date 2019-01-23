using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Social.Server
{
    public class Tokenned : IHttpActionResult
    {
        private readonly IHttpActionResult _result;
        private readonly string _token;

        public Tokenned(IHttpActionResult result, string token)
        {
            _result = result;
            _token = token;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await _result.ExecuteAsync(new CancellationToken());
            response.Headers.Remove("Token");
            response.Headers.Add("Token", _token);
            return response;
        }
    }
}