using Notifications.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Common.DAL
{
    public interface INotificationsRepository
    {
        Task Save(Notification notification, bool beenRead);

        Task<IEnumerable<Notification>> UnReadItemsOf(string userId);
    }
}
