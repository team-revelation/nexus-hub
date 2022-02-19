using System.Linq;
using Chats.Commands.Chats;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;
using Types.Users;

namespace Chats.Authorization.Chats
{
    public class UpdateChatAuthorizer : AbstractRequestAuthorizer<UpdateChatCommand>
    {
        private readonly IChatService _chatService;
        
        public UpdateChatAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(UpdateChatCommand request)
        {
            var chat = await _chatService.Get(request.Uuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                {
                    var isSameUser = request.Uuid == user.Uuid;
                    var member = chat.Members.FirstOrDefault(member => member.Uuid == user.Uuid);
                    var roles = chat.Roles.Where(r => member?.Roles.Contains(r.Uuid) == true).ToList();
                    var hasPrivilege = roles.Any(role => role.Privileges.Contains(Privilege.UpdateChat)) || chat.Members.Count <= 2 || chat.Members.All(m => m.Uuid != chat.Creator);

                    return member is not null && hasPrivilege && isSameUser;
                }
            });
        }
    }
}