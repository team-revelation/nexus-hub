using Exploring.Commands;
using Exploring.Filters;
using MediatR.Behaviors.Authorization;

namespace Exploring.Authorization
{
    public class CreateSubmissionAuthorizer : AbstractRequestAuthorizer<CreateSubmissionCommand>
    {
        public override void BuildPolicy(CreateSubmissionCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.User.Uuid == user.Uuid
            });
        }
    }
}