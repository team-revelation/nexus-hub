using System;
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
    public class VoteForOptionHandler : IRequestHandler<VoteForOptionCommand, VoteForOptionResponse>
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        
        public VoteForOptionHandler(IChatService chatService, INotificationService notificationService, IUserService userService)
        {
            _chatService = chatService;
            _notificationService = notificationService;
            _userService = userService;
        }
        
        public async Task<VoteForOptionResponse> Handle(VoteForOptionCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null)
                throw new ArgumentException("This chat does not exist, please try again.");

            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.MessageUuid);
            if (messageIndex is -1)
                throw new ArgumentException("This chat does not exist, please try again.");

            var message = chat.Messages[messageIndex];
            var poll = message.Polls.FirstOrDefault(poll => poll.Uuid == request.PollUuid);
            
            if (poll is null)
                throw new ArgumentException("This poll does not exist, please try again.");

            if (poll.Options.Count <= request.Vote)
                throw new ArgumentException("This is an invalid vote.");

            foreach (var option in poll.Options.Where(option => option.Votes.Contains(request.UserUuid)))
                option.Votes.Remove(request.UserUuid);
            
            poll.Options[request.Vote].Votes.Add(request.UserUuid);
            poll.Votes = poll.Options.SelectMany(o => o.Votes).Count();
            
            chat.Messages[messageIndex] = message;

            var users = (await _userService.Query(new []{ message.Creator, request.UserUuid })).ToList();
            var user = users.FirstOrDefault(u => u.Uuid == request.UserUuid);
            var creator = users.FirstOrDefault(u => u.Uuid == message.Creator);
            
            if (creator is not null) _notificationService.Push(creator.Devices, new Notification(NotificationType.EditMessage, "Voted on poll", $"{(user is null ? "Unknown" : user.Username.Trim())} voted on your poll"));
            await _chatService.Update(request.ChatUuid, chat);
            return new (chat, poll);
        }
    }
}