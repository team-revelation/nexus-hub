using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Types.Other;
using Types.Users;

namespace Types.Chats
{
    [FirestoreData]
    public record Chat
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string Avatar { get; set; }
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Creator { get; set; }
        [FirestoreProperty] public long Creation { get; set; }

        [FirestoreProperty] public List<Role> Roles { get; set; } = new();
        [FirestoreProperty] public List<Member> Members { get; set; } = new();
        [FirestoreProperty] public List<Message> Messages { get; set; } = new();
    }

    public record Typing(Guid UserUuid, bool IsTyping);
    public record Status(Guid ChatUuid,  List<Typing> Typing);
    public record JoinRequest(List<Guid> UserUuids);
}