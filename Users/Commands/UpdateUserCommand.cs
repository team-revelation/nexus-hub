using System;
using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class UpdateUserCommand : IRequest<User>
    {
        public Guid Uuid { get; }
        public User User { get; }
        
        public UpdateUserCommand(User user, Guid uuid)
        {
            User = user;
            Uuid = uuid;
        }
    }
}