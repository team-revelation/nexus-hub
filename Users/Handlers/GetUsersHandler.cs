using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Users;
using MediatR;
using Types.Users;
using Users.Queries;

namespace Users.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
    {
        private readonly IUserService _userService;

        public GetUsersHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.All();
        }
    }
}