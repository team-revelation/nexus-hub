using System;
using MediatR.Behaviors.Authorization;
using Users.Commands;
using Users.Filters;

namespace Users.Authorization
{
    public class RequestFriendAuthorizer : AbstractRequestAuthorizer<RequestFriendCommand>
    {
        public override void BuildPolicy(RequestFriendCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}