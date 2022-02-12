using MediatR.Behaviors.Authorization;
using Users.Filters;
using Users.Queries;

namespace Users.Authorization
{
    public class GetUserAuthorizer : AbstractRequestAuthorizer<GetUserQuery>
    {
        public override void BuildPolicy(GetUserQuery request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid || request.Email == user.Email
            });
        }
    }
}