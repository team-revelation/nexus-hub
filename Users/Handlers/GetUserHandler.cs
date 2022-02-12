using System.Threading;
using System.Threading.Tasks;
using Contracts.Users;
using MediatR;
using Types.Users;
using Users.Queries;

namespace Users.Handlers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly IUserService _userService;

        public GetUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var res = string.IsNullOrEmpty(request.Email) ? await _userService.Get(request.Uuid) : await _userService.Get(request.Email);
            return res;
        }
    }
}