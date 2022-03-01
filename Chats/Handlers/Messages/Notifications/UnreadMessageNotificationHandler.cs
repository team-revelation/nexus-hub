using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Messages.Notifications
{
    public class UnreadMessageNotificationHandler : INotificationHandler<UnreadMessageNotification>
    {
        private readonly IRedisService _redisService;
        
        public UnreadMessageNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(UnreadMessageNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("message_unread", notification.AsData()));
        }
    }
}