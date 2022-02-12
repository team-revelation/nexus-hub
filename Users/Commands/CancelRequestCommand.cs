using System;
using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class CancelRequestCommand : IRequest<User>
    {
        public Guid Uuid { get; }
        public Guid FriendUuid { get; }
        
        public CancelRequestCommand(Guid friendUuid, Guid uuid)
        {
            FriendUuid = friendUuid;
            Uuid = uuid;
        }
    }
}