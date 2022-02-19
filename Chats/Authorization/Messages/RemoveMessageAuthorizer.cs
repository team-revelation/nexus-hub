using System.Linq;
using Chats.Commands.Messages;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;
using Types.Users;

namespace Chats.Authorization.Messages
{
    public class RemoveMessageAuthorizer : AbstractRequestAuthorizer<RemoveMessageCommand>
    {
        private readonly IChatService _chatService;
        
        public RemoveMessageAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(RemoveMessageCommand request)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                {
                    var userMember = chat.Members.FirstOrDefault(m => m.Uuid == user.Uuid);
                    var roles = chat.Roles.Where(r => userMember?.Roles.Contains(r.Uuid) == true).ToList();
                    var hasPermission = roles.Any(r => r.Privileges.Contains(Privilege.RemoveMessage));
                    var isCreator = chat.Messages.First(m => m.Uuid == request.Uuid).Creator == user.Uuid;
                    return userMember is not null && (hasPermission || isCreator);
                }
            });
        }
    }
}