using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Contracts.Users;
using MediatR;
using Types.Chats;
using Types.Users;

namespace Chats.Handlers
{
    public class FillChatHandler : IRequestHandler<FillChatCommand, Chat>
    {
        private readonly IUserService _userService;

        public FillChatHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<Chat> Handle(FillChatCommand request, CancellationToken cancellationToken)
        {
            var newChat = request.Chat;
            
            var users = (await _userService.Query(request.Chat.Members.Select(m => m.Uuid).ToList())).ToList();
            newChat.Members = request.Chat.Members.Select(member => new Member(users.First(user => user.Uuid == member.Uuid), member.Roles)).ToList();
            
            foreach (var read in newChat.Messages.SelectMany(message => message.ReadUsers))
                read.User = newChat.Members.First(user => user.Uuid == read.UserUuid);

            return newChat;
        }
    }
}