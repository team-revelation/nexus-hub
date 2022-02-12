using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Contracts.Chats;
using Contracts.Users;
using MediatR;
using Types.Chats;

namespace Chats.Handlers
{
    public class ReactToMessageHandler : IRequestHandler<ReactToMessageCommand, Chat>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        
        public ReactToMessageHandler(IChatService chatService, IUserService userService)
        {
            _chatService = chatService;
            _userService = userService;
        }
        
        public async Task<Chat> Handle(ReactToMessageCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.Get(request.UserUuid);
            if (user is null)
                throw new ArgumentException("This user does not exist, please try again.");
            
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null)
                throw new ArgumentException("This chat does not exist, please try again.");

            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.MessageUuid);
            if (messageIndex is -1)
                throw new ArgumentException("This message does not exist, please try again.");

            var message = chat.Messages[messageIndex];
            var reactionIndex = message.Reactions.FindIndex(reaction => reaction.Emoji.Emoji == request.Emoji.Emoji);
            if (reactionIndex == -1)
            {
                if (message.Reactions == null)
                    message.Reactions = new List<Reaction> { new() { Emoji = request.Emoji, Users = { user.Uuid } } };
                else
                    message.Reactions.Add(new Reaction { Emoji = request.Emoji, Users = new () { user.Uuid }});
            }
            else if (message.Reactions[reactionIndex].Users.Contains(user.Uuid))
            {
                if (message.Reactions[reactionIndex].Users.Count <= 1)
                    message.Reactions.RemoveAt(reactionIndex);
                else
                    message.Reactions[reactionIndex].Users.Remove(user.Uuid);
            }
            else 
                message.Reactions[reactionIndex].Users.Add(user.Uuid);
            
            return await _chatService.Update(request.ChatUuid, chat);
        }
    }
}