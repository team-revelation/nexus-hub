using System.Threading;
using System.Threading.Tasks;
using Contracts.Users;
using MediatR;
using Types.Users;
using Users.Commands;

namespace Users.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly IUserService _userService;
        
        public UpdateUserHandler(IUserService userService)
        {
            _userService = userService;
        }
        
        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.Get(request.Uuid);
            var newUser = user with
            {
                Avatar = request.User.Avatar ?? user.Avatar,
                Username = request.User.Username ?? user.Username,
                Description = request.User.Description ?? user.Description,
                Interests = request.User.Interests ?? user.Interests,
                Devices = request.User.Devices ?? user.Devices,
            };

            return await _userService.Update(request.Uuid, newUser);
        }
    }
}