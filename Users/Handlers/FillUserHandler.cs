using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Chats;
using Contracts.Users;
using MediatR;
using Newtonsoft.Json;
using Types.Chats;
using Types.Users;
using Users.Commands;

namespace Users.Handlers
{
    public class FillUserHandler : IRequestHandler<FillUserCommand, User>
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public FillUserHandler(IUserService userService, IChatService chatService)
        {
            _userService = userService;
            _chatService = chatService;
        }
        
        public async Task<User> Handle(FillUserCommand request, CancellationToken cancellationToken)
        {
            var chats = (await _chatService.All(request.User.Uuid)).ToList();
            
            var friendUuids = request.User.Friends.Select(friend => friend.Uuid);
            var chatUserUuids = chats.Select(chat => chat.Members).SelectMany(members => members, (_, member) => member.Uuid);
            var uuids = chatUserUuids.Concat(friendUuids).Append(request.User.Uuid).ToList();

            var users = (await _userService.Query(uuids)).ToList();
            var newUser = request.User with
            {
                Friends = new List<Friend>(),
                Chats = chats,
            };

            for (var i = 0; i < request.User.Friends.Count; i++)
            {
                var user = users.FirstOrDefault(u => u.Uuid == request.User.Friends[i].Uuid);
                if (user == null)
                    continue;
                
                newUser.Friends.Add(new Friend(user, request.User.Friends[i].Type));
            }
            
            foreach (var chat in newUser.Chats)
            {
                for (var i = chat.Members.Count - 1; i >= 0; i--)
                {
                    var member = chat.Members[i];
                    if (users.All(u => u.Uuid != member.Uuid))
                    {
                        chat.Members.Remove(member);
                        continue;
                    } 
                    
                    chat.Members[i] = new Member(users.Find(u => u.Uuid == member.Uuid), member.Roles);
                }
                
                foreach (var message in chat.Messages)
                {
                    for (var i = message.ReadUsers.Count - 1; i >= 0; i--)
                    {
                        var read = message.ReadUsers[i];
                        var user = chat.Members.FirstOrDefault(member => member.Uuid == read.UserUuid);
                        if (user == null)
                        {
                            message.ReadUsers.Remove(read);
                            continue;
                        }
                        
                        read.User = user;
                    }
                }
            }
            
            return newUser;
        }
    }
}