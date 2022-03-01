using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Messages
{
    public record EditChecklistResponse(Chat Chat, Checklist Checklist);
    public class EditChecklistCommand : IRequest<EditChecklistResponse>
    {
        public Guid ChecklistUuid { get; }
        public Guid MessageUuid { get; }
        public Guid ChatUuid { get; }
        public Checklist Checklist { get; }
        
        public EditChecklistCommand(Guid checklistUuid, Guid messageUuid, Guid chatUuid, Checklist checklist)
        {
            MessageUuid = messageUuid;
            ChatUuid = chatUuid;
            Checklist = checklist;
            ChecklistUuid = checklistUuid;
        }
    }
}