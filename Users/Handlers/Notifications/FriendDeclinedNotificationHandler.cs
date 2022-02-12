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
    public class FriendDeclinedNotificationHandler : INotificationHandler<FriendDeclinedNotification>
    {
        private readonly IRedisService _redisService;
        
        public FriendDeclinedNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(FriendDeclinedNotification notification, CancellationToken cancellationToken)
        {
            await _redisService.Publish(notification.Friend.ToString("D"), new RedisData("friend_declined", notification.User.Uuid));
        }
    }
}