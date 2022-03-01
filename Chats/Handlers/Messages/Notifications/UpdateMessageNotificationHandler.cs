using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Messages.Notifications
{
    public class UpdateMessageNotificationHandler : INotificationHandler<UpdateMessageNotification>
    {
        private readonly IRedisService _redisService;
        
        public UpdateMessageNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(UpdateMessageNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("message_updated", notification.AsData()));
        }
    }
}