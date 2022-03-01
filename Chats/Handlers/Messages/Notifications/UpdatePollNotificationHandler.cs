using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Messages.Notifications
{
    public class UpdatePollNotificationHandler : INotificationHandler<UpdatePollNotification>
    {
        private readonly IRedisService _redisService;
        
        public UpdatePollNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(UpdatePollNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("poll_updated", notification.AsData()));
        }
    }
}