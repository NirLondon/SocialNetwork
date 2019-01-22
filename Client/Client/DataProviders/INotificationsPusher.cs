using Client.Models.ReturnedDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.DataProviders
{
    public interface INotificationsPusher
    {
        event Action<Notification> Pushed;

        Task RemoveNotification(Notification notification);
    }
}
