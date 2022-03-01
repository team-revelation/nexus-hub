using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Messages.Notifications
{
    public class UpdateChecklistNotificationHandler : INotificationHandler<UpdateChecklistNotification>
    {
        private readonly IRedisService _redisService;
        
        public UpdateChecklistNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(UpdateChecklistNotification notification, CancellationToken cancellationToken)
        {
            foreach (var member in notification.Chat.Members)
                await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("checklist_updated", notification.AsData()));
        }
    }
}