using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Contracts.Database
{
    public record SetRequest(string Type, object Data);
    
    public interface IDatabaseService
    {
        Task Create(string collection, string document, object obj, Type type);
        Task<List<T>> Query<T>(string collection, List<string> documents);
        Task<List<T>> All<T>(string collection);
        Task<T> Get<T>(string collection, string document);
        Task Update(string collection, string document, object newObj, Type type);
        Task Delete(string collection, string document);
        Task Clear(string collection);
    }
}