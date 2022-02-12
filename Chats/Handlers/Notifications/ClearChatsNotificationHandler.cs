using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Notifications;
using Contracts.Chats;
using Contracts.Redis;
using MediatR;

namespace Chats.Handlers.Notifications
{
    public class ClearChatsNotificationHandler : INotificationHandler<ClearChatsNotification>
    {
        private readonly IRedisService _redisService;
        
        public ClearChatsNotificationHandler(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Handle(ClearChatsNotification notification, CancellationToken cancellationToken)
        {
            var handledUsers = new List<Guid>();
            foreach (var chat in notification.Chats)
            {
                foreach (var member in chat.Members.Where(member => !handledUsers.Contains(member.Uuid)))
                {
                    await _redisService.Publish(member.Uuid.ToString("D"), new RedisData("chats_clear", null));
                    handledUsers.Add(member.Uuid);
                }
            }
        }
    }
}