using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Redis;
using Contracts.Users;
using MediatR;
using Types.Sockets;
using Types.Users;
using Users.Commands;
using Users.Notifications;

namespace Users.Handlers.Notifications
{
    public class FriendRequestedNotificationHandler : INotificationHandler<FriendRequestedNotification>
    {
        private readonly IRedisService _redisService;
        
        public FriendRequestedNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(FriendRequestedNotification notification, CancellationToken cancellationToken)
        {
            var friend = new Friend(notification.User, FriendType.Incoming);
            await _redisService.Publish(notification.Friend.ToString("D"), new RedisData("friend_requested", friend));
        }
    }
}