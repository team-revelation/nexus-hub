using MediatR.Behaviors.Authorization;
using Users.Commands;
using Users.Filters;

namespace Users.Authorization
{
    public class AcceptRequestAuthorizer : AbstractRequestAuthorizer<AcceptRequestCommand>
    {
        public override void BuildPolicy(AcceptRequestCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}