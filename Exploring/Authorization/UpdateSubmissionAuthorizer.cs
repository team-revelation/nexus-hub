using Exploring.Commands;
using Exploring.Filters;
using MediatR.Behaviors.Authorization;
using Types.Users;

namespace Exploring.Authorization
{
    public class UpdateSubmissionAuthorizer : AbstractRequestAuthorizer<UpdateSubmissionCommand>
    {
        public override void BuildPolicy(UpdateSubmissionCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.User.Uuid == user.Uuid
            });
        }
    }
}