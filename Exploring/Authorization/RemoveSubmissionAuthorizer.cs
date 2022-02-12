using Exploring.Commands;
using Exploring.Filters;
using MediatR.Behaviors.Authorization;

namespace Exploring.Authorization
{
    public class RemoveSubmissionAuthorizer : AbstractRequestAuthorizer<RemoveSubmissionCommand>
    {
        public override void BuildPolicy(RemoveSubmissionCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}