using System;
using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class DeclineRequestCommand : IRequest<User>
    {
        public Guid Uuid { get; }
        public Guid FriendUuid { get; }
        
        public DeclineRequestCommand(Guid friendUuid, Guid uuid)
        {
            FriendUuid = friendUuid;
            Uuid = uuid;
        }
    }
}