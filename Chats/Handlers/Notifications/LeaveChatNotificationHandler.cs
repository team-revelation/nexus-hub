using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;
using Types.Sockets;

namespace Chats.Handlers.Notifications
{
    public class LeaveChatNotificationHandler : INotificationHandler<LeaveChatNotification>
    {
        private readonly IRedisService _redisService;
        
        public LeaveChatNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(LeaveChatNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members.Where(member => member.Uuid != notification.UserUuid))
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("chat_kicked", notification.Chat));
            
            await _redisService.Publish(notification.UserUuid.ToString("D"), new RedisData("chat_left", notification.Chat));
        }
    }
}