using MediatR;
using Types.Users;

namespace Users.Notifications
{
    public class FriendUpdatedNotification : INotification
    {
        public User User { get; }
        
        public FriendUpdatedNotification(User user)
        {
            User = user;
        }
    }
}