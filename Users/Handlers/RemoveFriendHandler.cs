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
    public class RemoveFriendHandler : IRequestHandler<RemoveFriendCommand, User>
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public RemoveFriendHandler(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }
        
        public async Task<User> Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
        {
            var users = (await _userService.Query(new List<Guid> { request.FriendUuid, request.Uuid})).ToList();
            var friend = users.FirstOrDefault(u => u.Uuid == request.FriendUuid);
            var user = users.FirstOrDefault(u => u.Uuid == request.Uuid);

            if (friend is null || user is null)
                throw new ArgumentException("These users do not exist, please try again.");

            if (user.IsFriend(friend, FriendType.Friend))
            {
                friend.Friends.RemoveAll(u => u.Uuid == request.Uuid);
                user.Friends.RemoveAll(u => u.Uuid == request.FriendUuid);

                _notificationService.Push(friend.Devices, new Notification(NotificationType.FriendRemove, "Removed friend", $"{user.Username.Trim()} removed you from their friends list"));
                await _userService.Update(new Dictionary<Guid, User> { { request.Uuid, user }, { request.FriendUuid, friend } });
            }
            
            return user;
        }
    }
}