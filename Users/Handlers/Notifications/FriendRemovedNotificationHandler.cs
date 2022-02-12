using System.Threading;
using System.Threading.Tasks;
using Contracts.Redis;
using Contracts.Users;
using MediatR;
using Types.Sockets;
using Users.Commands;
using Users.Notifications;

namespace Users.Handlers.Notifications
{
    public class FriendRemovedNotificationHandler : INotificationHandler<FriendRemovedNotification>
    {
        private readonly IRedisService _redisService;
        
        public FriendRemovedNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(FriendRemovedNotification notification, CancellationToken cancellationToken)
        {
            await _redisService.Publish(notification.Friend.ToString("D"), new RedisData("friend_removed", notification.User.Uuid));
        }
    }
}