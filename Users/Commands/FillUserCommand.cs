using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class FillUserCommand : IRequest<User>
    {
        public User User { get; }
        
        public FillUserCommand(User user)
        {
            User = user;
        } 
    }
}