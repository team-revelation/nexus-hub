using MediatR.Behaviors.Authorization;
using Users.Commands;
using Users.Filters;

namespace Users.Authorization
{
    public class CancelRequestAuthorizer : AbstractRequestAuthorizer<CancelRequestCommand>
    {
        public override void BuildPolicy(CancelRequestCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}