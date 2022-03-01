using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Messages.Notifications
{
    public class RemoveMessageNotificationHandler : INotificationHandler<RemoveMessageNotification>
    {
        private readonly IRedisService _redisService;
        
        public RemoveMessageNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(RemoveMessageNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("message_removed", notification.AsData()));
        }
    }
}