using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Chats.Commands.Chats;
using Contracts.Chats;
using Contracts.Notifications;
using Contracts.Users;
using MediatR;
using Types.Chats;
using Types.Users;

namespace Chats.Handlers.Chats
{
    public class JoinChatHandler : IRequestHandler<JoinChatCommand, Chat>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public JoinChatHandler(IChatService chatService, IUserService userService, INotificationService notificationService)
        {
            _chatService = chatService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<Chat> Handle(JoinChatCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.Uuid);
            var users = (await _userService.Query(request.UserUuids)).ToList();

            foreach (var user in users)
            {
                if (chat is null || user is null) 
                    throw new ArgumentException("This chat or user does not exist, please try again.");

                chat.Members.Add(new Member(user, new List<Guid>()));
                
                if (request.UserUuids.Count == 1)
                {
                    var targets = users.Where(u => chat.Members.Any(m => m.Uuid == u.Uuid) && u.Uuid != user.Uuid);
                    var notification = new Notification(
                        NotificationType.CreateChat, 
                        "Added to chat", 
                        $"{user.Username} has been added to one of your chats."
                    );
                
                    _notificationService.Push(targets, notification);
                }
                
                _notificationService.Push(user.Devices, new Notification(NotificationType.CreateChat, "Added to chat", "You have been added to an existing chat."));
            }
            

            if (request.UserUuids.Count > 1)
            {
                var targets = users.Where(u => chat.Members.Any(m => m.Uuid == u.Uuid) && !request.UserUuids.Contains(u.Uuid));
                var notification = new Notification(
                    NotificationType.CreateChat, 
                    "Added to chat", 
                    "Multiple people have been added to one of your chats."
                );
                
                _notificationService.Push(targets, notification);
            }

            return await _chatService.Update(request.Uuid, chat);
        }
    }
}