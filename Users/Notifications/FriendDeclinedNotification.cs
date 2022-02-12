using System;
using MediatR;
using Types.Users;

namespace Users.Notifications
{
    public class FriendDeclinedNotification : INotification
    {
        public User User { get; }
        public Guid Friend { get; }
        
        public FriendDeclinedNotification(Guid friend, User user)
        {
            Friend = friend;
            User = user;
        }
    }
}