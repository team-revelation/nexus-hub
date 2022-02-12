using System;
using MediatR;

namespace Users.Commands
{
    public class RemoveUserCommand : IRequest
    {
        public Guid Uuid { get; }
        
        public RemoveUserCommand(Guid uuid)
        {
            Uuid = uuid;
        }
    }
}