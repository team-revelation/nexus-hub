using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication;
using Contracts.Users;
using MediatR.Behaviors.Authorization;
using Types.Users;

namespace Exploring.Filters
{
    public class MustHaveUserRequirement : IAuthorizationRequirement
    {
        public Func<User, bool> IsAuthorizedCheck { get; set; } 

        class MustHaveUserRequirementHandler : IAuthorizationHandler<MustHaveUserRequirement>
        {
            private readonly IUserService _userService;
            
            public MustHaveUserRequirementHandler(IUserService userService)
            {
                _userService = userService;
            }
            
            public async Task<AuthorizationResult> Handle(MustHaveUserRequirement request, CancellationToken cancellationToken)
            {
                var user = await _userService.Get(TokenAuthAttribute.CurrentEmail);
                var res = request.IsAuthorizedCheck(user) ? AuthorizationResult.Succeed() : AuthorizationResult.Fail();
                return res;
            }
        }
    }
}