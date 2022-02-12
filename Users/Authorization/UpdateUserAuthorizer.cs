using MediatR.Behaviors.Authorization;
using Users.Commands;
using Users.Filters;

namespace Users.Authorization
{
    public class UpdateUserAuthorizer : AbstractRequestAuthorizer<UpdateUserCommand>
    {
        public override void BuildPolicy(UpdateUserCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}