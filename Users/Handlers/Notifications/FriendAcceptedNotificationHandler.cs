using System.Threading;
using System.Threading.Tasks;
using Contracts.Redis;
using MediatR;
using Types.Users;
using Users.Notifications;

namespace Users.Handlers.Notifications
{
    public class FriendAcceptedNotificationHandler : INotificationHandler<FriendAcceptedNotification>
    {
        private readonly IRedisService _redisService;
        
        public FriendAcceptedNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(FriendAcceptedNotification notification, CancellationToken cancellationToken)
        {
            var friend = new Friend(notification.User, FriendType.Friend);
            await _redisService.Publish(notification.Friend.ToString("D"), new RedisData("friend_accepted", friend));
        }
    }
}