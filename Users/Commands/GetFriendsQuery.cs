using System;
using System.Collections.Generic;
using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class GetFriendsQuery : IRequest<IEnumerable<Friend>>
    {
        public Guid Uuid { get; }
        public bool Pending { get; }
        
        public GetFriendsQuery(Guid uuid, bool pending)
        {
            Uuid = uuid;
            Pending = pending;
        }
    }
}