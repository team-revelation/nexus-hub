using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Database.Firestore;

namespace Contracts.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IFirestoreService _firestoreService;
        public DatabaseService(IFirestoreService firestoreService)
        {
            _firestoreService = firestoreService;
        }

        public async Task Create<T>(string collection, string document, T obj)
        {
            await _firestoreService.Add(collection, document, obj);
        }

        public async Task<List<T>> Query<T>(string collection, List<string> documents)
        {
            var data = await _firestoreService.Query<T>(collection, documents);
            if (data is null) throw new NullReferenceException($"{collection} is null.");
            return data;
        }
        
        public async Task<List<T>> All<T>(string collection)
        {
            var data = await _firestoreService.All<T>(collection);
            if (data is null) throw new NullReferenceException($"{collection} is null.");
            return data;
        }

        public async Task<T> Get<T>(string collection, string document)
        {
            return await _firestoreService.Get<T>(collection, document);
        }

        public async Task Update<T>(string collection, string document, T newObj)
        {
            await _firestoreService.Update(collection, document, newObj);
        }

        public async Task Delete(string collection, string document)
        {
            await _firestoreService.Remove(collection, document);
        }

        public async Task Clear(string collection)
        {
            await _firestoreService.Clear(collection);
        }
    }
}