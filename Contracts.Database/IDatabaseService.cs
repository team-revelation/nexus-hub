using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Contracts.Database
{
    public interface IDatabaseService
    {
        Task Create<T>(string collection, string document, T obj);
        Task<List<T>> Query<T>(string collection, List<string> documents);
        Task<List<T>> All<T>(string collection);
        Task<T> Get<T>(string collection, string document);
        Task Update<T>(string collection, string document, T newObj);
        Task Delete(string collection, string document);
        Task Clear(string collection);
    }
}