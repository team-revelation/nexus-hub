using System.Threading;
using System.Threading.Tasks;
using Contracts.Users;
using MediatR;
using Users.Commands;

namespace Users.Handlers
{
    public class ClearUsersHandler : IRequestHandler<ClearUsersCommand>
    {
        private readonly IUserService _userService;
        
        public ClearUsersHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(ClearUsersCommand request, CancellationToken cancellationToken)
        {
            await _userService.Clear();
            return Unit.Value;
        }
    }
}