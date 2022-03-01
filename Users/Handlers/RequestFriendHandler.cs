using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Notifications;
using Contracts.Users;
using MediatR;
using Types.Users;
using Users.Commands;

namespace Users.Handlers
{
    public class RequestFriendHandler : IRequestHandler<RequestFriendCommand, User>
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        
        public RequestFriendHandler(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }
        
        public async Task<User> Handle(RequestFriendCommand request, CancellationToken cancellationToken)
        {
            var friend = request.FriendEmail is null ? await _userService.Get(request.FriendUuid) : await _userService.Get(request.FriendEmail);
            var user = await _userService.Get(request.Uuid);

            if (friend is null || user is null || friend == user)
                throw new ArgumentException("These users do not exist or they are not friends, please try again.");

            if (!user.IsFriend(friend))
            {
                if (!user.Friends.Any(f => f.Uuid == friend.Uuid || f.Email == friend.Email))
                    user.Friends.Add(new Friend(friend, FriendType.Outgoing));
                
                if (!friend.Friends.Any(f => f.Uuid == user.Uuid)) 
                    friend.Friends.Add(new Friend(user, FriendType.Incoming));

                _notificationService.Push(friend.Devices, new Notification(NotificationType.FriendRequest, "New friend request", $"{user.Username.Trim()} sent you a friend request"));
                await _userService.Update(new Dictionary<Guid, User> { { user.Uuid, user }, { friend.Uuid, friend } });
            }

            return user;
        }
    }
}