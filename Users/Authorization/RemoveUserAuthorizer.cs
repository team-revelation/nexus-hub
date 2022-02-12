using MediatR.Behaviors.Authorization;
using Users.Commands;
using Users.Filters;

namespace Users.Authorization
{
    public class RemoveUserAuthorizer : AbstractRequestAuthorizer<RemoveUserCommand>
    {
        public override void BuildPolicy(RemoveUserCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => user.Uuid == request.Uuid
            });
        }
    }
}