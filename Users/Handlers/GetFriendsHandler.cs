using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Users;
using MediatR;
using Types.Users;
using Users.Commands;

namespace Users.Handlers
{
    public class GetFriendsHandler : IRequestHandler<GetFriendsQuery, IEnumerable<Friend>>
    {
        private readonly IUserService _userService;
        
        public GetFriendsHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<IEnumerable<Friend>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
        {
            var friends = (await _userService.Get(request.Uuid)).Friends;
            if (request.Pending)
                friends.RemoveAll(friend => friend.Type == FriendType.Friend);

            return friends;
        }
    }
}