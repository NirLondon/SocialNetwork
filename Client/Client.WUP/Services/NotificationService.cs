using Client.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WUP.Services
{
    public class NotificationService : INotificationService
    {
        public void GoToNotification()
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {
            MainPageService.Instance.LogOut();
        }
    }
}
