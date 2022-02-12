using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Chats;
using Contracts.Exploring;
using Contracts.Users;
using MediatR;
using Types.Users;
using Users.Commands;

namespace Users.Handlers
{
    public class RemoveUserHandler: IRequestHandler<RemoveUserCommand>
    {
        private readonly IUserService _userService;
        private readonly IExploreService _exploreService;
        private readonly IChatService _chatService;
        
        public RemoveUserHandler(IUserService userService, IChatService chatService, IExploreService exploreService)
        {
            _userService = userService;
            _chatService = chatService;
            _exploreService = exploreService;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.Get(request.Uuid);
            if (user is null)
                throw new ArgumentException("this user does not exist, please try again.");

            var friendUsers = new Dictionary<Guid, User>();
            if (user.Friends.Any())
            {
                var users = (await _userService.Query(user.Friends.Select(friend => friend.Uuid).ToList())).ToList();
                foreach (var friend in user.Friends)
                {
                    var friendUser = users.FirstOrDefault(u => u.Uuid == friend.Uuid);
                    if (friendUser is null) continue;
                    
                    friendUser.Friends.RemoveAll(f => f.Uuid == user.Uuid);
                    friendUsers.Add(friend.Uuid, friendUser);
                }

                await _userService.Update(friendUsers);
            }

            var chats = (await _chatService.All(request.Uuid)).ToList();
            if (chats.Any())
            {
                foreach (var chat in chats)
                {
                    chat.Members.RemoveAll(member => member.Uuid == request.Uuid);
                    foreach (var message in chat.Messages)
                    {
                        foreach (var option in message.Polls.SelectMany(poll => poll.Options))
                            option.Votes.RemoveAll(vote => vote == request.Uuid);

                        message.ReadUsers.RemoveAll(read => read.UserUuid == request.Uuid);
                    }
                }
            
                await _chatService.Update(chats.ToDictionary(c => c.Uuid, c => c));
            }
            
            try { await _exploreService.Delete(request.Uuid); }
            catch (Exception _) { /* ignored */ }
                
            await _userService.Delete(request.Uuid);
            return Unit.Value;
        }
    }
}