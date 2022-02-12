using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Chats;
using Contracts.Users;
using MediatR;
using Types.Chats;
using Types.Users;
using Users.Commands;

namespace Users.Handlers
{
    public class FillUsersHandler : IRequestHandler<FillUsersCommand, IEnumerable<User>>
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        
        public FillUsersHandler(IUserService userService, IChatService chatService)
        {
            _userService = userService;
            _chatService = chatService;
        }
        
        public async Task<IEnumerable<User>> Handle(FillUsersCommand request, CancellationToken cancellationToken)
        {
            var users = (await _userService.All()).ToList();
            var newUsers = new List<User>();

            foreach (var user in request.Users)
            {
                var newUser = user with
                {
                    Friends = new List<Friend>(),
                    Chats = new List<Chat>(),
                };

                for (var i = 0; i < user.Friends.Count; i++)
                {
                    var friend = users.FirstOrDefault(u => u.Uuid == user.Friends[i].Uuid);
                    if (friend == null) continue;
                    
                    newUser.Friends.Add(new Friend(friend, user.Friends[i].Type));
                }
                
                newUser.Chats = (await _chatService.All(user.Uuid)).ToList();
                foreach (var chat in newUser.Chats)
                {
                    chat.Members = chat.Members.Select(member => new Member(users.Find(u => u.Uuid == member.Uuid), member.Roles)).ToList();
                    
                    foreach (var message in chat.Messages)
                    {
                        foreach (var read in message.ReadUsers)
                        {
                            var member = chat.Members.FirstOrDefault(member => member.Uuid == read.UserUuid);
                            if (member == null) continue;
                            
                            read.User = member;
                        }
                    }
                }
                
                newUsers.Add(newUser);
            }

            return newUsers;
        }
    }
}