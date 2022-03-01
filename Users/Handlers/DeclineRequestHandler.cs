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
    public class DeclineRequestHandler : IRequestHandler<DeclineRequestCommand, User>
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
 
        public DeclineRequestHandler(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }
        
        public async Task<User> Handle(DeclineRequestCommand request, CancellationToken cancellationToken)
        {
            var users = (await _userService.Query(new List<Guid> { request.FriendUuid, request.Uuid})).ToList();
            var friend = users.FirstOrDefault(user => user.Uuid == request.FriendUuid);
            var user = users.FirstOrDefault(user => user.Uuid == request.Uuid);

            if (friend is null || user is null)
                throw new ArgumentException("The friend or user uuid is invalid, please try again.");

            if (!user.IsFriend(friend, FriendType.Outgoing) && !user.IsFriend(friend, FriendType.Incoming))
                throw new ArgumentException("These users are not pending friends, please try again.");

            user.Friends.Remove(user.Friends.Find(userFriend => userFriend.Uuid == request.FriendUuid));
            friend.Friends.Remove(friend.Friends.Find(userFriend => userFriend.Uuid == request.Uuid));

            _notificationService.Push(user.Devices, new Notification(NotificationType.FriendDecline, "Friend request declined", $"{friend.Username.Trim()} declined your friend request"));
           
            await _userService.Update(new Dictionary<Guid, User> { { request.Uuid, user }, { friend.Uuid, friend } });
            return user;
        }
    }
}