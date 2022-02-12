using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Types.Other;

namespace Types.Users
{
    [FirestoreData]
    public record Role
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string Color { get; set;  }
        [FirestoreProperty] public bool Administrator { get; set; } = false;
        [FirestoreProperty] public List<Privilege> Privileges { get; set; } = new ();
        
        public Role(string name, string color, List<Privilege> privileges)
        {
            Uuid = Guid.NewGuid();
            Name = name;
            Color = color;
            Privileges = privileges;
        }
        
        public Role(string name, string color, bool administrator, List<Privilege> privileges)
        {
            Uuid = Guid.NewGuid();
            Name = name;
            Color = color;
            Administrator = administrator;
            Privileges = privileges;
        }

        public Role () { }

        public static Role Admin() => new("Admin", "#a55eea", true, Enum.GetValues<Privilege>().ToList());
    }

    [FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<Privilege>))]
    public enum Privilege
    {
        AddUser,
        RemoveUser,
        RemoveChat,
        UpdateChat,
        RemoveMessage,
    }
    
    [FirestoreData]
    public record Member : IUserBase
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public List<Interest> Interests { get; set; } = new ();
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public List<Guid> Roles { get; set; } = new ();

        public Member(IUserBase user, IEnumerable<Guid> roles)
        {
            Username = user.Username;
            Uuid = user.Uuid;
            Email = user.Email;
            Avatar = user.Avatar;
            Description = user.Description;
            Interests = user.Interests;
            Roles = roles.ToList();
        }

        public Member() { }
    }
}