using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Contracts.Redis
{
    public class RedisService : IRedisService
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IConnectionMultiplexer _redis;
        
        public RedisService(IConnectionMultiplexer redis)
        {
            _serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            _redis = redis;
        }

        public async Task Subscribe (string channel, Action<RedisData> onMessage)
        {
            var sub = _redis.GetSubscriber();
            var queue = await sub.SubscribeAsync(channel);
            
            queue.OnMessage(message =>
                {
                    var value = JsonConvert.DeserializeObject<RedisData>(message.Message, _serializerSettings);
                    onMessage(value);
                }
            );
        }
        
        public async Task Unsubscribe(string channel)
        {
            var sub = _redis.GetSubscriber();
            await sub.UnsubscribeAsync(channel);
        }

        public async Task<long> Publish (string channel, RedisData data)
        {
            var sub = _redis.GetSubscriber();
            var value = JsonConvert.SerializeObject(data, _serializerSettings);
            return await sub.PublishAsync(channel, value);
        }
    }
}