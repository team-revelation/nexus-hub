using Chats.Commands;
using Chats.Filters;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization
{
    public class ReactToMessageAuthorizer : AbstractRequestAuthorizer<ReactToMessageCommand>
    {
        public override void BuildPolicy(ReactToMessageCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => user.Uuid == request.UserUuid
            });
        }
    }
}