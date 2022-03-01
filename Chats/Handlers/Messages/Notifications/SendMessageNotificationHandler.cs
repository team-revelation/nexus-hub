using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Messages.Notifications
{
    public class SendMessageNotificationHandler : INotificationHandler<SendMessageNotification>
    {
        private readonly IRedisService _redisService;
        
        public SendMessageNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(SendMessageNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("message_created", notification.AsData()));
        }
    }
}