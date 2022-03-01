using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Chats.Notifications.Chats;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Chats.Notifications
{
    public class UpdateChatNotificationHandler : INotificationHandler<UpdateChatNotification>
    {
        private readonly IRedisService _redisService;
        
        public UpdateChatNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(UpdateChatNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("chat_updated", notification.Chat));
        }
    }
}