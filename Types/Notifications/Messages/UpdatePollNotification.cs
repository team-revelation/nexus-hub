using System;
using Types.Chats;

namespace Types.Notifications.Messages
{
    public record UpdatePollNotificationData
    {
        public UpdatePollNotificationData(Guid chatUuid, Guid messageUuid, Poll poll)
        {
            ChatUuid = chatUuid;
            MessageUuid = messageUuid;
            Poll = poll;
        }
        
        public Guid ChatUuid { get; }
        public Guid MessageUuid { get; }
        public Poll Poll { get; }
    }
}