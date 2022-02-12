using System.Threading;
using System.Threading.Tasks;
using Contracts.Redis;
using MediatR;
using Types.Sockets;
using Types.Users;
using Users.Notifications;

namespace Users.Handlers.Notifications
{
    public class UserUpdatedNotificationHandler : INotificationHandler<UserUpdatedNotification>
    {
        private readonly IRedisService _redisService;
        
        public UserUpdatedNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(UserUpdatedNotification notification, CancellationToken cancellationToken)
        {
            await _redisService.Publish(notification.User.Uuid.ToString("D"), new RedisData("user_updated", notification.User));
        }
    }
}