using System;
using MediatR;
using Types.Chats;
using Types.Notifications.Messages;

namespace Chats.Notifications
{
    public record UpdatePollNotification : INotification
    {
        public Chat Chat { get; set; }
        public Guid MessageUuid { get; set; }
        public Poll Poll { get; set; }
        
        public UpdatePollNotification(Chat chat, Guid messageUuid, Poll poll)
        {
            Chat = chat;
            MessageUuid = messageUuid;
            Poll = poll;
        }
        
        public UpdatePollNotificationData AsData()
        {
            return new UpdatePollNotificationData(Chat.Uuid, MessageUuid, Poll);
        }
    }
}