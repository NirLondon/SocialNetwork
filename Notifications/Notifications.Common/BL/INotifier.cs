using Notifications.Common.Models;

namespace Notifications.Common.BL
{
    public interface INotifier
    {
        void Notify(Notification notification);
    }
}
