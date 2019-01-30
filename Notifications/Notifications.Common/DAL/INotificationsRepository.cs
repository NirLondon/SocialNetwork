using Notifications.Common.Models.ReceivedDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Common.DAL
{
    public interface INotificationsRepository
    {
        Task Save(Notification notification, bool beenRead);

        Task<IEnumerable<Notification>> NotificationsOf(string userId, bool includeRead, DateTime stratTime);

        void MarkAsRead(int notificationId);
    }
}
