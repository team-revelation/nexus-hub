using System;
using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class RemoveFriendCommand : IRequest<User>
    {
        public Guid Uuid { get; }
        public Guid FriendUuid { get; }
        
        public RemoveFriendCommand(Guid friendUuid, Guid uuid)
        {
            FriendUuid = friendUuid;
            Uuid = uuid;
        }
    }
}