using MediatR;
using Types.Chats;

namespace Chats.Notifications
{
    public record TypingNotification : INotification
    {
        public Status Status { get; set; }
        public Chat Chat { get; set; }

        public TypingNotification(Status status, Chat chat)
        {
            Status = status;
            Chat = chat;
        }
    }
}