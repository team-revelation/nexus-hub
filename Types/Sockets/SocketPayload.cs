using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace Types.Sockets
{
    public class SocketPayload
    {
        private object Data { get; }
        private string Type { get; }
        
        public SocketPayload(string type, object data)
        {
            Type = type;
            Data = data;
        }

        public override string ToString()
        {
            if (Data == null) return "{\"type\":\"" + Type + "\"}";

            var data = JsonConvert.SerializeObject(Data, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return "{\"type\":\"" + Type + "\",\"data\":" + data +"}";
        }
    }
}