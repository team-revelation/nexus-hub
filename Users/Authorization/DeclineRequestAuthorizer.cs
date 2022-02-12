using MediatR.Behaviors.Authorization;
using Users.Commands;
using Users.Filters;

namespace Users.Authorization
{
    public class DeclineRequestAuthorizer : AbstractRequestAuthorizer<DeclineRequestCommand>
    {
        public override void BuildPolicy(DeclineRequestCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}