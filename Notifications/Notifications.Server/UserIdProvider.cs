using Microsoft.AspNet.SignalR;

namespace Notifications.Server
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request) => request.Headers.Get("UserID");
    }
}