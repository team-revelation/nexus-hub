using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Types.Chats;
using Types.Other;

namespace Types.Users
{
    public interface IUserBase
    {
        Guid Uuid { get; }
        string Username { get; }
        string Email { get; }
        string Avatar { get; }
        string Description { get; }
        List<Interest> Interests { get; }
    }

    [FirestoreData]
    public record Interest
    {
        [FirestoreProperty] public string Title { get; set; }
        [FirestoreProperty] public string Icon { get; set; }
    };
    
    [FirestoreData]
    public record User : IUserBase
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; set; }
        [FirestoreProperty] public string Username { get; set; }
        [FirestoreProperty] public string Email { get; set; }
        [FirestoreProperty] public string Avatar { get; set; }
        [FirestoreProperty] public string Description { get; set; }
        [FirestoreProperty] public List<Interest> Interests { get; set; } = new ();
        
        [FirestoreProperty] public List<string> Devices { get; set; } = new();

        public List<Chat> Chats { get; set; } = new();
        [FirestoreProperty] public List<Friend> Friends { get; set; } = new();

        public bool IsFriend(User friend, FriendType type)
        {
            var reverse = type switch
                          {
                              FriendType.Incoming => FriendType.Outgoing,
                              FriendType.Outgoing => FriendType.Incoming,
                              _ => FriendType.Friend
                          };
            
            var friendHasUser = friend.Friends.Any(f => f.Uuid == friend.Uuid && f.Type == reverse);
            var userHasFriend = Friends.Any(f => f.Uuid == friend.Uuid && f.Type == type);

            return userHasFriend || friendHasUser;
        }
        
        public bool IsFriend(User friend)
        {
            var friendHasUser = friend.Friends.Any(f => f.Uuid == friend.Uuid);
            var userHasFriend = Friends.Any(f => f.Uuid == friend.Uuid);

            return userHasFriend || friendHasUser;
        }
    }
}