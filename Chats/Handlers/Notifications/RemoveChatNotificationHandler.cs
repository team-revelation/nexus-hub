using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;
using Types.Sockets;

namespace Chats.Handlers.Notifications
{
    public class RemoveChatNotificationHandler : INotificationHandler<RemoveChatNotification>
    {
        private readonly IRedisService _redisService;
        
        public RemoveChatNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(RemoveChatNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("chat_removed", notification.Chat));
        }
    }
}