using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Contracts.Chats;
using Contracts.Notifications;
using Contracts.Users;
using Google.Cloud.Firestore;
using MediatR;
using Types.Chats;
using Types.Users;

namespace Chats.Handlers
{
    public class CreateChatHandler : IRequestHandler<CreateChatCommand, Chat>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        
        public CreateChatHandler(IChatService chatService, IUserService userService, INotificationService notificationService)
        {
            _chatService = chatService;
            _userService = userService;
            _notificationService = notificationService;
        }

        private IEnumerable<Member> GetMembers(IEnumerable<User> users)
        {
            return users.Select(user => new Member(user, new List<Guid>())).ToList();
        }

        public async Task<Chat> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var adminRole = Role.Admin();
            var creator = new Member(await _userService.Get(request.Chat.Creator), new List<Guid> { adminRole.Uuid });
            var newChat = request.Chat with
            {
                Uuid = Guid.NewGuid(),
                Creation = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Members = new List<Member> { creator },
                Roles = new List<Role> { adminRole }
            };

            if (request.MemberUuids is not null && request.MemberUuids.Any())
            {
                var memberUsers = (await _userService.Query(request.MemberUuids.ToList())).ToList();
                newChat.Members.AddRange(GetMembers(memberUsers));
                
                foreach (var member in memberUsers)
                    _notificationService.Push(member.Devices, new Notification(NotificationType.CreateChat, "Added to chat", "You have been added to a new chat."));
            }
            
            var chat = await _chatService.Create(newChat);
            return chat;
        }
    }
}