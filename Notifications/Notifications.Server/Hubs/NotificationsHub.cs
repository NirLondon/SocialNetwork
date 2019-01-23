using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Notifications.Common.BL;
using Notifications.Common.DAL;
using Notifications.Common.Models;

namespace Notifications.Server.Hubs
{
    public class NotificationsHub : Hub, INotifier
    {
        private static readonly Dictionary<string, string> userIdsConnetionIds;
        private readonly INotificationsRepository _repository;

        public NotificationsHub(INotificationsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        static NotificationsHub()
        {
            userIdsConnetionIds = new Dictionary<string, string>();
        }

        public override async Task OnConnected()
        {
            var userId = Context.Headers.Get("UserID");
            userIdsConnetionIds.Add(userId, Context.ConnectionId);
            Clients.Caller.OnNotifications((await _repository.UnReadItemsOf(userId)).ToArray());
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
                userIdsConnetionIds.Remove(Context.Headers.Get("UserID"));
            else userIdsConnetionIds.Remove();
        }

        public void Notify(Notification notification)
        {
            var clientNotif = new ClientNotification { Data = notification.Data, Type = notification.Type };

            foreach (var userId in notification.ReceiversIds)
            {
                bool beenRead;
                if (beenRead = userIdsConnetionIds.TryGetValue(userId, out string connectionId))
                    Clients.Client(connectionId).OnNotifications(new[] { clientNotif });
                 _repository.Save(notification, beenRead);
            }
        }
    }
}