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

namespace Chats.Handlers
{
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, Chat>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public SendMessageHandler(IChatService chatService, IUserService userService, INotificationService notificationService)
        {
            _chatService = chatService;
            _userService = userService;
            _notificationService = notificationService;
        }
        
        public async Task<Chat> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null || chat.Members.All(member => member.Uuid != request.Message.Creator))
                throw new ArgumentException("This chat does not exist, please try again.");

            var creator = chat.Members.FirstOrDefault(member => member.Uuid == request.Message.Creator);
            var read = creator is null ? new List<Read>() : new List<Read> { new (creator) };
            
            var creation = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var newMessage = request.Message with
            {
                Creation = creation,
                Edited = creation,
                ReadUsers = read,
                Uuid = request.Message.Uuid == Guid.Empty ? Guid.NewGuid() : request.Message.Uuid
            };

            chat.Messages.Add(newMessage);
            await _chatService.Update(request.ChatUuid, chat);

            var users = (await _userService.Query(chat.Members.Select(m => m.Uuid).ToList())).ToList();
            var otherChatUsers = users.Where(u => u.Uuid != newMessage.Creator);
            _notificationService.Push(otherChatUsers, new Notification($"New message from {users.FirstOrDefault(u => u.Uuid == newMessage.Creator)?.Username ?? "Unknown"}", newMessage));

            return chat;
        }
    }
}