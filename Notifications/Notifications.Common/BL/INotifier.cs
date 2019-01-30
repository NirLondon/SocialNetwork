using Notifications.Common.Models.ReceivedDTOs;

namespace Notifications.Common.BL
{
    public interface INotifier
    {
        void Notify(Notification notification);
    }
}
