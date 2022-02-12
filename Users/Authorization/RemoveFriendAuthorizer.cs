using MediatR.Behaviors.Authorization;
using Users.Commands;
using Users.Filters;

namespace Users.Authorization
{
    public class RemoveFriendAuthorizer : AbstractRequestAuthorizer<RemoveFriendCommand>
    {
        public override void BuildPolicy(RemoveFriendCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}