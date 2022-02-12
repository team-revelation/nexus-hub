using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Types.Other;
using Types.Users;

namespace Types.Exploring
{
    [FirestoreData]
    public record Position
    {
        [FirestoreProperty] public double Latitude { get; set; }
        [FirestoreProperty] public double Longitude { get; set; }

        public Position(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        
        public Position () { }
    }
    
    [FirestoreData]
    public record Submission
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; set; }
        [FirestoreProperty] public string Username { get; set; }
        [FirestoreProperty] public string Avatar { get; set; }
        
        [FirestoreProperty] public string Description { get; set; }
        [FirestoreProperty] public List<Interest> Interests { get; set; } = new ();
        [FirestoreProperty] public Position Position { get; set; }

        public Submission(IUserBase user, Position position)
        {
            Uuid = user.Uuid;
            Username = user.Username;
            Avatar = user.Avatar;
            Description = user.Description;
            Interests = user.Interests;
            Position = position;
        }
        
        public Submission () { }
    }
}