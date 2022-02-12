﻿using System.Linq;
using Chats.Commands;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization
{
    public class SendMessageAuthorizer : AbstractRequestAuthorizer<SendMessageCommand>
    {
        private readonly IChatService _chatService;
        
        public SendMessageAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(SendMessageCommand request)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                                    {
                                        var isMessageCreator = request.Message.Creator == user.Uuid;
                                        var isMember = chat.Members.Any(member => member.Uuid == user.Uuid);
                                        return isMessageCreator && isMember;
                                    }
            });
        }
    }
}