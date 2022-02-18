using System.Linq;
using Chats.Commands.Chats;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;
using Types.Users;

namespace Chats.Authorization.Chats
{
    public class JoinChatAuthorizer : AbstractRequestAuthorizer<JoinChatCommand>
    {
        private readonly IChatService _chatService;
        
        public JoinChatAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        } 
        
        public override async void BuildPolicy(JoinChatCommand request)
        {
            var chat = await _chatService.Get(request.Uuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                                    {
                                        var member = chat.Members.FirstOrDefault(member => member.Uuid == user.Uuid);
                                        var roles = chat.Roles.Where(r => member?.Roles.Contains(r.Uuid) == true).ToList();
                                        var hasPermission = roles.Any(role => role.Privileges.Contains(Privilege.AddUser)) || chat.Members.Count <= 2 || chat.Members.All(m => m.Uuid != chat.Creator);
                                        return member is not null && hasPermission;
                                    }
            });
        }
    }
}