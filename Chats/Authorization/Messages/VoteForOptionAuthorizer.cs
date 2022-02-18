using System.Linq;
using Chats.Commands.Messages;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Messages
{
    public class VoteForOptionAuthorizer : AbstractRequestAuthorizer<VoteForOptionCommand>
    {
        private readonly IChatService _chatService;
        
        public VoteForOptionAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(VoteForOptionCommand request)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                                    {
                                        var isMember = chat?.Members.Any(member => member.Uuid == request.UserUuid);
                                        var isVoter = user.Uuid == request.UserUuid;
                                        return isMember == true && isVoter;
                                    }
            });
        }
    }
}