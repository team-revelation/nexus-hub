using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Memorystore
{
    public interface IMemorystoreService
    {
        Task Create<T>(string coll, string key, T data);
        Task<T> Get<T>(string coll, string key, Func<string, Task<T>> fallback);
        IEnumerable<T> All<T>(string pattern = "*");
        Task Update<T>(string coll, string key, T data);
        Task Delete(string coll, string key);
    }
}