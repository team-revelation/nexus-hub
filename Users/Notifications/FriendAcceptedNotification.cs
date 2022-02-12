using System;
using MediatR;
using Types.Users;

namespace Users.Notifications
{
    public class FriendAcceptedNotification : INotification
    {
        public User User { get; }
        public Guid Friend { get; }
        
        public FriendAcceptedNotification(Guid friend, User user)
        {
            Friend = friend;
            User = user;
        }
    }
}