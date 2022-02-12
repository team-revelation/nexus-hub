using System.Collections.Generic;
using MediatR;
using Types.Users;

namespace Users.Commands
{
    public class FillUsersCommand : IRequest<IEnumerable<User>>
    {
        public IEnumerable<User> Users { get; }
        
        public FillUsersCommand(IEnumerable<User> users)
        {
            Users = users;
        } 
    }
}