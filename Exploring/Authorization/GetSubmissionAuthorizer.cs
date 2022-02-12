using Exploring.Filters;
using Exploring.Queries;
using MediatR.Behaviors.Authorization;

namespace Exploring.Authorization
{
    public class GetSubmissionAuthorizer : AbstractRequestAuthorizer<GetSubmissionQuery>
    {
        public override void BuildPolicy(GetSubmissionQuery request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}