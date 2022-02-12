using System;
using System.Threading.Tasks;

namespace Contracts.Redis
{
    public interface IRedisService
    {
        Task Subscribe(string channel, Action<RedisData> onMessage);
        Task Unsubscribe(string channel);
        Task<long> Publish(string channel, RedisData data);
    }
}