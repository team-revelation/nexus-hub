using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Types.Other;

namespace Types.Chats
{
    [FirestoreData]
    public record Message
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; init; }
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Creator { get; init; }
        [FirestoreProperty] public string Content { get; init; }
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Reference { get; init; }
        [FirestoreProperty] public List<Reaction> Reactions { get; set; } = new();
        [FirestoreProperty] public List<Checklist> Checklists { get; init; } = new();
        [FirestoreProperty] public List<Poll> Polls { get; init; } = new();
        [FirestoreProperty] public List<Attachment> Attachments { get; init; } = new ();
        [FirestoreProperty] public List<Embed> Embeds { get; init; } = new ();
        [FirestoreProperty] public List<Read> ReadUsers { get; init; } = new();
        [FirestoreProperty] public long Creation { get; init; }
        [FirestoreProperty] public long Edited { get; init; }
    }

    [FirestoreData]
    public record Emoticon
    {
        [FirestoreProperty] public string Emoji { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public string V { get; set; } // Version
    }
    
    [FirestoreData]
    public record Reaction
    {
        [FirestoreProperty] public Emoticon Emoji { get; set; }
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public List<Guid> Users { get; set; }
    }

    [FirestoreData]
    public record Checklist
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; set; }
        [FirestoreProperty] public string Title { get; set; }
        [FirestoreProperty] public List<Item> Items { get; set; }
    }

    [FirestoreData]
    public record Item
    {
        [FirestoreProperty] public string Title { get; set; }
        [FirestoreProperty] public Attachment Attachment { get; set; }
        [FirestoreProperty] public bool Done { get; set; }
    }

    [FirestoreData]
    public record Poll
    {
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public Guid Uuid { get; init; }
        [FirestoreProperty] public string Title { get; set; }
        [FirestoreProperty] public int Votes { get; set; }
        [FirestoreProperty] public List<Option> Options { get; set; }
    }

    [FirestoreData]
    public record Option
    {
        [FirestoreProperty] public string Title { get; set; }
        [FirestoreProperty(ConverterType = typeof(GuidConverter))] public List<Guid> Votes { get; set; }
    }

    [FirestoreData(ConverterType = typeof(FirestoreEnumNameConverter<AttachmentType>))]
    public enum AttachmentType
    {
        Image,
        Video,
        Audio,
        File
    }
    
    [FirestoreData]
    public record Attachment
    {
        [FirestoreProperty] public AttachmentType Type { get; set; }
        [FirestoreProperty] public string Url { get; set; }
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public int Size { get; set; }
        
        [FirestoreProperty] public int Length { get; set; }
        
        [FirestoreProperty] public int Width { get; set; }
        [FirestoreProperty] public int Height { get; set; }
    }
    
    [FirestoreData]
    public record Embed
    {
        [FirestoreProperty] public string Description { get; init; }
        [FirestoreProperty] public Image Image { get; init; }
        [FirestoreProperty] public string Link { get; init; }
        [FirestoreProperty] public string Title { get; init; }
    }
}