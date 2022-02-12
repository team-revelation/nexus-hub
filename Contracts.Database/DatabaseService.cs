using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace Contracts.Database
{
    public class DatabaseService : IDatabaseService
    {
        public async Task Create(string collection, string document, object obj, Type type)
        {
            await "http://firestore-service"
                  .AppendPathSegments("api", "create", collection, document)
                  .PostJsonAsync(new SetRequest(type.AssemblyQualifiedName, obj));
        }

        public async Task<List<T>> Query<T>(string collection, List<string> documents)
        {
            return await "http://firestore-service"
                        .AppendPathSegments("api", "query", collection)
                        .SetQueryParam("documents", documents)
                        .GetJsonAsync<List<T>>();
        }
        
        public async Task<List<T>> All<T>(string collection)
        {
            try
            {
                return await "http://firestore-service"
                            .AppendPathSegments("api", "all", collection)
                            .GetJsonAsync<List<T>>();
            }
            catch (Exception)
            {
                return new List<T> ();
            }
        }

        public async Task<T> Get<T>(string collection, string document)
        {
            try 
            {
                return await "http://firestore-service"
                            .AppendPathSegments("api", "get", collection, document)
                            .GetJsonAsync<T>();
            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task Update(string collection, string document, object newObj, Type type)
        {
            await "http://firestore-service"
                 .AppendPathSegments("api", "update", collection, document)
                 .PutJsonAsync(new SetRequest(type.AssemblyQualifiedName, newObj));
        }

        public async Task Delete(string collection, string document)
        {
            await "http://firestore-service"
                 .AppendPathSegments("api", "remove", collection, document)
                 .DeleteAsync();
        }

        public async Task Clear(string collection)
        {
            await "http://firestore-service"
                 .AppendPathSegments("api", "clear", collection)
                 .DeleteAsync();
        }
    }
}