using System.Collections.Generic;
using MediatR;
using Types.Users;

namespace Users.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<User>>
    {
        
    }
}