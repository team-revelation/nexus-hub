using Google.Cloud.Firestore;

namespace Types.Chats
{
    [FirestoreData]
    public record Image
    {
        [FirestoreProperty] public string Url { get; init; }
        [FirestoreProperty] public int Height { get; init; }
        [FirestoreProperty] public int Width { get; init; }
    }
}