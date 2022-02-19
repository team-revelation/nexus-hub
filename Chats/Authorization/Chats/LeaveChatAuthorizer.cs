using System.Linq;
using Chats.Commands.Chats;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;
using Types.Users;

namespace Chats.Authorization.Chats
{
    public class LeaveChatAuthorizer : AbstractRequestAuthorizer<LeaveChatCommand>
    {
        private readonly IChatService _chatService;
        
        public LeaveChatAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        } 
        
        public override async void BuildPolicy(LeaveChatCommand request)
        {
            var chat = await _chatService.Get(request.Uuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                {
                    var member = chat.Members.FirstOrDefault(m => m.Uuid == user.Uuid);
                    var isSameUser = request.UserUuid == user.Uuid;
                    var roles = chat.Roles.Where(r => member?.Roles.Contains(r.Uuid) == true).ToList();
                    var hasPrivileges = roles.Any(r => r.Privileges.Contains(Privilege.RemoveUser));
                    
                    return member is not null && (isSameUser || hasPrivileges);
                }
            });
        }
    }
}