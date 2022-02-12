using MediatR.Behaviors.Authorization;
using Users.Commands;
using Users.Filters;

namespace Users.Authorization
{
    public class GetFriendsAuthorizerAuthorizer : AbstractRequestAuthorizer<GetFriendsQuery>
    {
        public override void BuildPolicy(GetFriendsQuery request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}