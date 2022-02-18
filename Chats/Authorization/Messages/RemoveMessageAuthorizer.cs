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
                                        var userMember = chat.Members.FirstOrDefault(member => member.Uuid == user.Uuid);
                                        var roles = chat.Roles.Where(r => userMember?.Roles.Contains(r.Uuid) == true).ToList();
                                        var hasPermission = roles.Any(role => role.Privileges.Contains(Privilege.RemoveMessage));
                                        var isCreator = chat.Messages.First(message => message.Uuid == request.Uuid).Creator == user.Uuid;
                                        return userMember is not null && (hasPermission || isCreator);
                                    }
            });
        }
    }
}