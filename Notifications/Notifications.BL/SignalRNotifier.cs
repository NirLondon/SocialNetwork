using Microsoft.AspNet.SignalR;
using Notifications.Common.BL;
using Notifications.Common.DAL;
using Notifications.Common.Models.ReceivedDTOs;
using Notifications.Common.Models.ReturnedDTOs;
using System.Linq;

namespace Notifications.BL
{
    public class SignalRNotifier : INotifier
    {
        private readonly IHubContext _hubContext;
        private readonly INotificationsRepository _repository;

        public SignalRNotifier(INotificationsRepository repository)
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext("NotificationsHub");
            _repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public void Notify(Notification notification)
        {
            if (!notification.ReceiversIds.Any()) return;
            var clientNotif = new ReturnedNotification
            {
                Type = notification.Type,
                Data = notification.Data
            };

            _repository.Save(notification, false);

            foreach (var userId in notification.ReceiversIds)
                _hubContext.Clients.User(userId).OnNotificarion(clientNotif);
        }
    }
}