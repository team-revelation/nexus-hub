using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Chats.Commands.Messages;
using Contracts.Chats;
using Contracts.Notifications;
using Contracts.Users;
using MediatR;
using Types.Chats;

namespace Chats.Handlers.Messages
{
    public class ReactToMessageHandler : IRequestHandler<ReactToMessageCommand, ReactToMessageResponse>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public ReactToMessageHandler(IChatService chatService, IUserService userService, INotificationService notificationService)
        {
            _chatService = chatService;
            _userService = userService;
            _notificationService = notificationService;
        }
        
        public async Task<ReactToMessageResponse> Handle(ReactToMessageCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null)
                throw new ArgumentException("This chat does not exist, please try again.");

            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.MessageUuid);
            if (messageIndex is -1)
                throw new ArgumentException("This message does not exist, please try again.");

            var message = chat.Messages[messageIndex];

            var users = (await _userService.Query(new[] { request.UserUuid, message.Creator })).ToList();
            var user = users.FirstOrDefault(u => u.Uuid == request.UserUuid);
            var creator = users.FirstOrDefault(u => u.Uuid == message.Creator);
            if (user is null)
                throw new ArgumentException("This user does not exist, please try again.");
            
            var reactionIndex = message.Reactions.FindIndex(reaction => reaction.Emoji.Emoji == request.Emoji.Emoji);
            if (reactionIndex == -1)
            {
                if (message.Reactions == null) message.Reactions = new List<Reaction> { new() { Emoji = request.Emoji, Users = { user.Uuid } } };
                else message.Reactions.Add(new Reaction { Emoji = request.Emoji, Users = new () { user.Uuid }});
                
                if (creator is not null) _notificationService.Push(creator.Devices, new Notification(NotificationType.EditMessage, "Reacted to message", $"{user.Username.Trim()} reacted to your message with '{request.Emoji.Emoji}'"));
            }
            else if (message.Reactions[reactionIndex].Users.Contains(user.Uuid))
            {
                if (message.Reactions[reactionIndex].Users.Count <= 1)
                    message.Reactions.RemoveAt(reactionIndex);
                else
                    message.Reactions[reactionIndex].Users.Remove(user.Uuid);
            }
            else
            {
                message.Reactions[reactionIndex].Users.Add(user.Uuid);
                if (creator is not null) _notificationService.Push(creator.Devices, new Notification(NotificationType.EditMessage, "Reacted to message", $"{user.Username.Trim()} reacted to your message with '{request.Emoji.Emoji}'"));
            }
            
            return new(await _chatService.Update(request.ChatUuid, chat), message);
        }
    }
}