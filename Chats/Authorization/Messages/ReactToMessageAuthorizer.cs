using Chats.Commands.Messages;
using Chats.Filters;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Messages
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