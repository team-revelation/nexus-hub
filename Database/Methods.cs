using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Database
{
    public class Methods
    {
        public static async Task<bool> Add(string coll, string doc, object data)
        {
            var match = Program.Store.Collection(coll).Document(doc);
            await match.CreateAsync(data);
            return true;
        }

        public static async Task<List<DocumentSnapshot>> Query (string coll, IEnumerable<string> docs)
        {
            var collection = Program.Store.Collection(coll);
            var query = collection.WhereIn(FieldPath.DocumentId, docs);
            var snap = await query.GetSnapshotAsync();
            return snap.Documents.ToList();
        }

        public static async Task<DocumentSnapshot> Get(string coll, string doc)
        {
            var match = Program.Store.Collection(coll).Document(doc);
            var snap = await match.GetSnapshotAsync();
            
            return !snap.Exists ? null : snap;
        }

        public static async Task<List<DocumentSnapshot>> All(string coll)
        {
            var match = Program.Store.Collection(coll);
            var collectionSnapshot = await match.GetSnapshotAsync();

            var docs = collectionSnapshot.Documents.ToList();
            return docs.Count == 0 ? null : docs;
        }
        
        public static async Task<bool> Update(string coll, string doc, object @new)
        {
            var match = Program.Store.Collection(coll).Document(doc);
            var snap = await match.GetSnapshotAsync();
                
            if (!snap.Exists) return false;
                
            await match.SetAsync(@new);
            return true;
        }
        
        public static async Task<bool> Remove(string coll, string doc)
        {
            var match = Program.Store.Collection(coll).Document(doc);
            var snap = await match.GetSnapshotAsync();

            if (!snap.Exists) return false;
                
            await match.DeleteAsync();
            return true;
        }
        
        public static async Task<bool> Clear(string coll)
        {
            var match = Program.Store.Collection(coll);
            var snap = await match.GetSnapshotAsync();

            foreach (var doc in snap.Documents)
                await doc.Reference.DeleteAsync();

            return true;
        }
    }
}