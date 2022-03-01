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
    public class AcceptRequestHandler : IRequestHandler<AcceptRequestCommand, User>
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
 
        public AcceptRequestHandler(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }
        
        public async Task<User> Handle(AcceptRequestCommand request, CancellationToken cancellationToken)
        {
            var users = (await _userService.Query(new List<Guid> { request.FriendUuid, request.Uuid})).ToList();
            var friend = users.FirstOrDefault(user => user.Uuid == request.FriendUuid);
            var user = users.FirstOrDefault(user => user.Uuid == request.Uuid);

            if (friend is null || user is null)
                throw new ArgumentException("The friend or user uuid is invalid, please try again.");

            if (!user.IsFriend(friend, FriendType.Outgoing) && !user.IsFriend(friend, FriendType.Incoming))
                throw new ArgumentException("These users are not pending friends, please try again.");

            var friendIndex = user.Friends.FindIndex(userFriend => userFriend.Uuid == request.FriendUuid);
            var userIndex = friend.Friends.FindIndex(userFriend => userFriend.Uuid == request.Uuid);

            if (friendIndex is -1 || userIndex is -1)
                throw new ArgumentException("These users are not friends, please try again.");
            
            user.Friends[friendIndex].Type = FriendType.Friend;
            friend.Friends[userIndex].Type = FriendType.Friend;
            
            _notificationService.Push(user.Devices, new Notification(NotificationType.FriendAccept, "Friend request accepted", $"{friend.Username.Trim()} accepted your friend request"));
            await _userService.Update(new Dictionary<Guid, User> { { request.Uuid, user }, { friend.Uuid, friend } });
            return user;
        }
    }
}