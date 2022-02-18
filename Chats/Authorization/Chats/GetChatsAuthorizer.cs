using System;
using Chats.Filters;
using Chats.Queries;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Chats
{
    public class GetChatsAuthorizer : AbstractRequestAuthorizer<GetChatsQuery>
    {
        public override void BuildPolicy(GetChatsQuery request)
        {
            if (request.Uuid == Guid.Empty) return;
            
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Uuid == user.Uuid
            });
        }
    }
}