using MediatR;
using Types.Users;

namespace Users.Notifications
{
    public class UserUpdatedNotification : INotification
    {
        public User User { get; }
        
        public UserUpdatedNotification(User user)
        {
            User = user;
        }
    }
}