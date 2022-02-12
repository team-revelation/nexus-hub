using System.Threading;
using System.Threading.Tasks;
using Contracts.Redis;
using MediatR;
using Types.Sockets;
using Types.Users;
using Users.Notifications;

namespace Users.Handlers.Notifications
{
    public class FriendUpdatedNotificationHandler : INotificationHandler<FriendUpdatedNotification>
    {
        private readonly IRedisService _redisService;
        
        public FriendUpdatedNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }
        
        public async Task Handle(FriendUpdatedNotification notification, CancellationToken cancellationToken)
        {
            var friend = new Friend(notification.User, FriendType.Friend);
            foreach (var user in notification.User.Friends)
                await _redisService.Publish(user.Uuid.ToString("D"), new RedisData("friend_updated", friend));
        }
    }
}