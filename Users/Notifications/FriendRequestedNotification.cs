using System;
using MediatR;
using Types.Users;

namespace Users.Notifications
{
    public class FriendRequestedNotification : INotification
    {
        public User User { get; }
        public Guid Friend { get; }
        
        public FriendRequestedNotification(Guid friendUuid, User user)
        {
            Friend = friendUuid;
            User = user;
        }
    }
}