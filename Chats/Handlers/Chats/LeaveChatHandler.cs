using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication;
using Chats.Commands;
using Chats.Commands.Chats;
using Contracts.Chats;
using Contracts.Notifications;
using Contracts.Users;
using MediatR;
using Types.Chats;

namespace Chats.Handlers.Chats
{
    public class LeaveChatHandler : IRequestHandler<LeaveChatCommand, Chat>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public LeaveChatHandler(IChatService chatService, IUserService userService, INotificationService notificationService)
        {
            _chatService = chatService;
            _userService = userService;
            _notificationService = notificationService;
        }
        
        public async Task<Chat> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.Uuid);
            var users = (await _userService.Query(chat.Members.Select(m => m.Uuid).ToList())).ToList();
            var target = users.FirstOrDefault(u => u.Uuid == request.UserUuid);
            
            if (chat is null || target is null)
                throw new ArgumentException("This chat does not exist or this user is not a member of this chat, please don't try again.");
            
            chat.Members = chat.Members.Where(member => member.Uuid != request.UserUuid).ToList();

            foreach (var message in chat.Messages)
            {
                if (message.Polls is { Count: > 0 })
                {
                    foreach (var poll in message.Polls)
                    {
                        var hasVoted = false;
                        foreach (var option in poll.Options)
                        {
                            if (option.Votes.Contains(request.UserUuid))
                                hasVoted = true;
                            
                            option.Votes.RemoveAll(vote => vote == request.UserUuid);
                        }
                        
                        if (hasVoted)
                            poll.Votes--;
                    }
                }

                message.ReadUsers.RemoveAll(read => read.UserUuid == request.UserUuid);
            }

            var isKickerUser = TokenAuthAttribute.CurrentEmail == target.Email;
            var targets = !isKickerUser
                              ? users.Where(u => u.Uuid == request.UserUuid || u.Email != TokenAuthAttribute.CurrentEmail)
                              : users.Where(u => u.Uuid != request.UserUuid);
            
            foreach (var t in targets)
            {
                var notification = new Notification(
                    NotificationType.RemoveChat, 
                    $"{(isKickerUser ? "Left" : "Kicked from")} chat", 
                    $"{(t.Uuid == request.UserUuid ? "You have" : $"{target.Username} has")} {(isKickerUser ? "left" : "been kicked from")} one of your chats."
                );
                
                _notificationService.Push(t.Devices, notification);
            }
            
            await _chatService.Update(request.Uuid, chat);
            return chat;
        }
    }
}