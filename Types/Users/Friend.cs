using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Types.Other;

namespace Types.Users
{
    [FirestoreData]
    public enum FriendType
    {
        Friend,
        Incoming,
        Outgoing,
    }
    
    [FirestoreData]
    public record Friend : IUserBase
    {
        [FirestoreProperty(ConverterType = typeof(FirestoreEnumNameConverter<FriendType>))] public FriendType Type { get; set; }
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        
        public string Description { get; set; }
        public List<Interest> Interests { get; set; } = new ();
        
        public Friend (IUserBase user, FriendType type)
        {
            Type = type;
            Username = user.Username;
            Uuid = user.Uuid;
            Email = type == FriendType.Friend ? user.Email : null;
            Avatar = user.Avatar;
            Description = user.Description;
            Interests = user.Interests;
        }
        
        public Friend () { }
    }
}