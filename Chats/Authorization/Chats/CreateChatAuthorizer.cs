using Chats.Commands.Chats;
using Chats.Filters;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Chats
{
    public class CreateChatAuthorizer : AbstractRequestAuthorizer<CreateChatCommand>
    {
        public override void BuildPolicy(CreateChatCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Chat.Creator == user.Uuid
            });
        }
    }
}