using System.Collections.Generic;
using Types.Users;

namespace Contracts.Notifications
{
    public interface INotificationService
    {
        void Push (IEnumerable<User> users, Notification notification);
        void Push (IEnumerable<string> devices, Notification notification);
    }
}