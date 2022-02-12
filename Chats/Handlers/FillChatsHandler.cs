using System.Collections.Generic;
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
    public class FillChatsHandler : IRequestHandler<FillChatsCommand, IEnumerable<Chat>>
    {
        private readonly IUserService _userService;
        
        public FillChatsHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<IEnumerable<Chat>> Handle(FillChatsCommand request, CancellationToken cancellationToken)
        {
            var newChats = new List<Chat>();

            foreach (var chat in request.Chats)
            {
                var users = (await _userService.Query(chat.Members.Select(m => m.Uuid).ToList())).ToList();
                chat.Members = chat.Members.Select(member => new Member(users.First(user => user.Uuid == member.Uuid), member.Roles)).ToList();
                
                foreach (var read in chat.Messages.SelectMany(message => message.ReadUsers))
                    read.User = chat.Members.First(user => user.Uuid == read.UserUuid);
                newChats.Add(chat);
            }

            return newChats;
        }
    }
}