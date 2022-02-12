using System;
using MediatR;
using Types.Users;

namespace Users.Notifications
{
    public class FriendRemovedNotification : INotification
    {
        public User User { get; }
        public Guid Friend { get; }
        
        public FriendRemovedNotification(Guid friend, User user)
        {
            Friend = friend;
            User = user;
        }
    }
}