using MediatR;
using Types.Exploring;
using Types.Users;

namespace Exploring.Commands
{
    public class CreateSubmissionRequest 
    {
        public User User { get; set; }
        public Position Position { get; set; }
    }
    
    public class CreateSubmissionCommand : IRequest<Submission>
    {
        public User User { get; }
        public Position Position { get; }
        
        public CreateSubmissionCommand(User user, Position position)
        {
            User = user;
            Position = position;
        }
    }
}