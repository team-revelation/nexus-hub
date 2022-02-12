using System;
using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class AcceptRequestCommand : IRequest<User>
    {
        public Guid Uuid { get; }
        public Guid FriendUuid { get; }
        
        public AcceptRequestCommand(Guid friendUuid, Guid uuid)
        {
            FriendUuid = friendUuid;
            Uuid = uuid;
        }
    }
}