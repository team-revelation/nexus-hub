using System;
using System.Text.Json.Serialization;
using Google.Cloud.Firestore;
using Types.Other;
using Types.Users;

namespace Types.Chats
{
    [FirestoreData]
    public record Read
    {
        public Member User { get; set; }
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] [JsonIgnore] public Guid UserUuid { get; set; }
        [FirestoreProperty] public long Timestamp { get; init; }

        public Read(Member user)
        {
            User = user;
            UserUuid = user.Uuid;
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        
        public Read () { }
    }
}