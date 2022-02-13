using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FirebaseAdmin.Messaging;
using Types.Users;

namespace Contracts.Notifications
{
    public class NotificationService : INotificationService
    {
        private static string GetIcon(NotificationType type)
        {
            return type switch
            {
                NotificationType.SendMessage => "ic_message_create",
                NotificationType.EditMessage => "ic_message_edit",
                NotificationType.RemoveMessage => "ic_message_remove",
                NotificationType.CreateChat => "ic_chat_create",
                NotificationType.RemoveChat => "ic_chat_remove",
                NotificationType.FriendRequest => "ic_friend_request",
                NotificationType.FriendAccept => "ic_friend_accept",
                NotificationType.FriendDecline => "ic_friend_decline",
                NotificationType.FriendRemove => "ic_friend_remove",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void Push(IEnumerable<User> users, Notification notification)
        {
            var devices = new List<string>();
            foreach (var user in users)
                devices.AddRange(user.Devices);

            Push(devices, notification);
        }

        public void Push(IEnumerable<string> devices, Notification notification)
        {
            var list = devices.ToList();
            
            switch (list.Count)
            {
                case < 1:
                    break;
                case 1:
                {
                    var singleMessage = new Message
                    {
                        Notification = notification.Object,
                        Token = list[0],
                        Android = new AndroidConfig
                        {
                            Notification = new AndroidNotification
                            {
                                Icon = GetIcon(notification.Type),
                                Color = notification.Color,
                            }
                        }
                    };
                    
                    if (!string.IsNullOrEmpty(notification.Object.ImageUrl))
                    {
                        singleMessage.Apns = new ApnsConfig
                        {
                            Aps = new Aps
                            {
                                MutableContent = true,
                            },
                            FcmOptions = new ApnsFcmOptions
                            {
                                ImageUrl = notification.Object.ImageUrl,
                            }
                        };
                    }
                
                    FirebaseMessaging.DefaultInstance.SendAsync(singleMessage);
                    break;
                }
                default:
                {
                    var multiMessage = new MulticastMessage
                    {
                        Notification = notification.Object,
                        Tokens = list,
                        Android = new AndroidConfig
                        {
                            Notification = new AndroidNotification
                            {
                                Icon = GetIcon(notification.Type),
                                Color = notification.Color,
                            }
                        },
                    };

                    if (!string.IsNullOrEmpty(notification.Object.ImageUrl))
                    {
                        multiMessage.Apns = new ApnsConfig
                        {
                            Aps = new Aps
                            {
                                MutableContent = true,
                            },
                            FcmOptions = new ApnsFcmOptions
                            {
                                ImageUrl = notification.Object.ImageUrl
                            },
                        };
                    }
                    
                    FirebaseMessaging.DefaultInstance.SendMulticastAsync(multiMessage);
                    break;
                }
            }
        }
    }
}