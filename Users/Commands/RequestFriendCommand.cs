using System;
using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class RequestFriendCommand : IRequest<User>
    {
        public Guid Uuid { get; }
        public Guid FriendUuid { get; }
        public string FriendEmail { get; }
        
        public RequestFriendCommand(Guid friendUuid, Guid uuid)
        {
            FriendUuid = friendUuid;
            Uuid = uuid;
        }
        
        public RequestFriendCommand(string friendEmail, Guid uuid)
        {
            FriendEmail = friendEmail;
            Uuid = uuid;
        }
    }
}