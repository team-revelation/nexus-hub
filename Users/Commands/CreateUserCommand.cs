using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        public User User { get; }
        
        public CreateUserCommand(User user)
        {
            User = user;
        }   
    }
}