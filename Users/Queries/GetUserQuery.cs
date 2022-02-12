using System;
using MediatR;
using Types.Users;

namespace Users.Queries
{
    public class GetUserQuery : IRequest<User>
    {
        public Guid Uuid { get; }
        public string Email { get; }

        public GetUserQuery(Guid uuid)
        {
            Uuid = uuid;
        }
        
        public GetUserQuery(string email)
        {
            Email = email;
        }
    }
}