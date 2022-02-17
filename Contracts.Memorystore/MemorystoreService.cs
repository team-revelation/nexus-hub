using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Contracts.Memorystore
{
    public class MemorystoreService : IMemorystoreService
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IConnectionMultiplexer _memorystore;
        
        public MemorystoreService(IConnectionMultiplexer memorystore)
        {
            _serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            _memorystore = memorystore;
        }
        
        public async Task Create<T>(string coll, string key, T data)
        {
            var db = _memorystore.GetDatabase();
            if (!await db.KeyExistsAsync(key)) 
                await db.StringSetAsync($"{coll}.{key}", JsonConvert.SerializeObject(data, _serializerSettings));
        }
        
        public async Task<T> Get<T>(string coll, string key, Func<string, Task<T>> getFromDatabase)
        {
            var db = _memorystore.GetDatabase();
            if (!await db.KeyExistsAsync($"{coll}.{key}"))
                await Create(coll, key, await getFromDatabase(key));

            return JsonConvert.DeserializeObject<T>(db.StringGet($"{coll}.{key}"));
        }

        private T Get<T>(string key)
        {
            var db = _memorystore.GetDatabase();
            return JsonConvert.DeserializeObject<T>(db.StringGet(key));
        }
        
        public IEnumerable<T> All<T>(string coll)
        {
            var endpoints = _memorystore.GetEndPoints();
            var server = _memorystore.GetServer(endpoints.First());
            var db = _memorystore.GetDatabase();
            
            var keys = server.Keys(db.Database, $"{coll}.*");
            return keys.Select(key => Get<T>(key));
        }
        
        public async Task Update<T>(string coll, string key, T data)
        {
            var db = _memorystore.GetDatabase();
            if (!await db.KeyExistsAsync($"{coll}.{key}")) return;
            
            await db.StringSetAsync($"{coll}.{key}", JsonConvert.SerializeObject(data, _serializerSettings));
        }
        
        public async Task Delete(string coll, string key)
        {
            var db = _memorystore.GetDatabase();
            if (!await db.KeyExistsAsync($"{coll}.{key}")) return;

            await db.KeyDeleteAsync($"{coll}.{key}");
        }
    }
}