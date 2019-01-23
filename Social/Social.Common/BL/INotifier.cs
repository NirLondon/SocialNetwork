using Social.Common.Models.NotificationsDTOs;
using System.Threading.Tasks;

namespace Social.Common.BL
{
    public interface INotifier
    {
        Task NotifyLike(Like like);
    }
}
