using System.Threading;
using System.Threading.Tasks;
using Contracts.Users;
using MediatR;
using Types.Users;
using Users.Commands;

namespace Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUserService _userService;
        
        public CreateUserHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.Create(request.User);
        }
    }
}