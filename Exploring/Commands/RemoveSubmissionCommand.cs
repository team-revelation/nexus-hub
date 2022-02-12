using System;
using MediatR;

namespace Exploring.Commands
{
    public class RemoveSubmissionCommand : IRequest
    {
        public Guid Uuid { get; }
        
        public RemoveSubmissionCommand(Guid uuid)
        {
            Uuid = uuid;
        }
    }
}