using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Other.Notifications
{
    public class TypingNotificationHandler : INotificationHandler<TypingNotification>
    {
        private readonly IRedisService _redisService;

        public TypingNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(TypingNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("typing_updated", notification.Status));
        }
    }
}