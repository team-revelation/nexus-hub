using System;
using MediatR;
using Types.Exploring;
using Types.Users;

namespace Exploring.Commands
{
    public class UpdateSubmissionRequest
    {
        public User User { get; set; }
        public Position Position { get; set; }
    }
    
    public class UpdateSubmissionCommand : IRequest<Submission>
    {
        public User User { get; }
        public Position Position  { get; }
        
        public UpdateSubmissionCommand(User user, Position position)
        {
            User = user;
            Position = position;
        }
    }
}