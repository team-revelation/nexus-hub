using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Contracts.Redis
{
    public interface IRedisService
    {
        void Subscribe(string channel, Action<RedisChannel, RedisValue> onMessage);
        void Subscribe(string channel, Action<RedisData> onMessage);
        Task Unsubscribe(string channel);
        Task<long> Publish(string channel, RedisData data);
    }
}