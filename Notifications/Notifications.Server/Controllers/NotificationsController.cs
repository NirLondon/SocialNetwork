using Notifications.Common.BL;
using Notifications.Common.Models.ReceivedDTOs;
using System.Web.Http;

namespace Notifications.Server.Controllers
{
    [RoutePrefix("api/Notify")]
    public class NotificationsController : ApiController
    {
        private readonly INotifier _notifier;

        public NotificationsController(INotifier notifier)
        {
            _notifier = notifier;
        }

        [HttpPost]
        public void Notify(Notification notification)
        {
            _notifier.Notify(notification);
        }
    }
}
