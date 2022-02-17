using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Contracts.Database.Firestore
{
    public interface IFirestoreService
    {
        public Task<bool> Add<T>(string coll, string doc, T data);

        public Task<List<T>> Query<T>(string coll, IEnumerable<string> docs);

        public Task<T> Get<T>(string coll, string doc);

        public Task<List<T>> All<T>(string coll);

        public Task<bool> Update<T>(string coll, string doc, T @new);

        public Task<bool> Remove(string coll, string doc);

        public Task<bool> Clear(string coll);
    }
}