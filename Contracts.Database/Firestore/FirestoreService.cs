using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Memorystore;
using Google.Cloud.Firestore;

namespace Contracts.Database.Firestore
{
    public class FirestoreService : IFirestoreService
    {
        private readonly IMemorystoreService _memorystore;
        public FirestoreService(IMemorystoreService memorystore)
        {
            _memorystore = memorystore;
        }

        public async Task<bool> Add<T>(string coll, string doc, T data)
        {
            await _memorystore.Create(coll, doc, data);
            var match = Database.Store.Collection(coll).Document(doc);
            await match.CreateAsync(data);
            return true;
        }

        public async Task<List<T>> Query<T>(string coll, IEnumerable<string> docs)
        {
            // Ten is the current limit (22/02/2022) of firebase equality clauses, so we have to batch them for some reason.
            // Thank you so much Google, I really appreciate it.
            var batches = docs
                         .Select((x, i) => new { Index = i, Value = x })
                         .GroupBy(x => x.Index / 10) 
                         .Select(x => x.Select(v => v.Value).ToList())
                         .ToList();
            
            var collection = Database.Store.Collection(coll);
            var results = new List<T>();
            foreach (var batch in batches)
            {
                var query = collection.WhereIn(FieldPath.DocumentId, batch);
                var snap = await query.GetSnapshotAsync();
                results.AddRange(snap.Documents.Select(d => d.ConvertTo<T>()));
            }
            
            return results;
        }

        public async Task<T> Get<T>(string coll, string doc)
        {
            return await _memorystore.Get(coll, doc, async s =>
            {
                var match = Database.Store.Collection(coll).Document(doc);
                var snap = await match.GetSnapshotAsync();

                return !snap.Exists ? default : snap.ConvertTo<T>();
            });
        }

        public async Task<List<T>> All<T>(string coll)
        {
            var cache = _memorystore.All<T>(coll).ToList();
            if (cache.Count > 0) return cache;
            
            var match = Database.Store.Collection(coll);
            var collectionSnapshot = await match.GetSnapshotAsync();

            var docs = collectionSnapshot.Documents.ToList();
            return docs.Count == 0 ? null : docs.Select(d => d.ConvertTo<T>()).ToList();
        }
        
        public async Task<bool> Update<T>(string coll, string doc, T @new)
        {
            await _memorystore.Update(coll, doc, @new);
            
            var match = Database.Store.Collection(coll).Document(doc);
            var snap = await match.GetSnapshotAsync();
                
            if (!snap.Exists) return false;
                
            await match.SetAsync(@new);
            return true;
        }
        
        public async Task<bool> Remove(string coll, string doc)
        {
            await _memorystore.Delete(coll, doc);
            var match = Database.Store.Collection(coll).Document(doc);
            var snap = await match.GetSnapshotAsync();

            if (!snap.Exists) return false;
                
            await match.DeleteAsync();
            return true;
        }
        
        public async Task<bool> Clear(string coll)
        {
            var match = Database.Store.Collection(coll);
            var snap = await match.GetSnapshotAsync();

            foreach (var doc in snap.Documents)
            {
                await _memorystore.Delete(coll, doc.Id);
                await doc.Reference.DeleteAsync();
            }

            return true;
        }
    }
}