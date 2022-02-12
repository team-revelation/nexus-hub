using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Database.Controllers
{
    [ApiController]
    [Route("api")]
    public class Controller : ControllerBase
    {
        private static readonly JsonSerializerSettings SerializerSettings = new ()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };
        
        private static readonly JsonSerializerSettings DeserializerSettings = new ()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.All,
        };

        public record SetRequest(string Type, JsonDocument Data);
        
        [HttpPost("create/{coll}/{doc}")]
        public async Task<IActionResult> Create(string coll, string doc, [FromBody] SetRequest request)
        {
            var t = Type.GetType(request.Type);
            if (t == null) throw new NullReferenceException($"{request.Type} is an invalid type.");
            var raw = request.Data.RootElement.ToString();
            if (raw == null) throw new NullReferenceException("This JSON data is malformed.");

            var @new = JsonConvert.DeserializeObject(raw, t, DeserializerSettings);
            await Methods.Add(coll, doc, @new);
            return Ok(JsonConvert.SerializeObject(@new, SerializerSettings));
        }
        
        [HttpGet("query/{coll}")]
        public async Task<IActionResult> Query(string coll, [FromQuery] List<string> documents)
        {
            var docs = await Methods.Query(coll, documents);
            if (docs is null) return Problem($"Object not found: {coll}");
            
            var data = docs.Select(document => document.ToDictionary());
            return Ok(JsonConvert.SerializeObject(data, SerializerSettings));
        }
        
        [HttpGet("all/{coll}")]
        public async Task<IActionResult> All(string coll)
        {
            var documents = await Methods.All(coll);
            if (documents is null) return Problem($"Object not found: {coll}");
            
            var data = documents.Select(document => document.ToDictionary());
            return Ok(JsonConvert.SerializeObject(data, SerializerSettings));
        }
        
        [HttpGet("get/{coll}/{doc}")]
        public async Task<IActionResult> Get(string coll, string doc)
        {
            var document = await Methods.Get(coll, doc);
            return document is null 
                ? Problem($"Object not found: {coll}/{doc}") 
                : Ok(JsonConvert.SerializeObject(document.ToDictionary(), SerializerSettings));
        }
        
        [HttpPut("update/{coll}/{doc}")]
        public async Task<IActionResult> Update(string coll, string doc, [FromBody] SetRequest request)
        {
            var t = Type.GetType(request.Type);
            if (t == null) throw new NullReferenceException("This is an invalid type.");
            var raw = request.Data.RootElement.ToString();
            if (raw == null) throw new NullReferenceException("This JSON data is malformed.");

            var @new = JsonConvert.DeserializeObject(raw, t, DeserializerSettings);
            await Methods.Update(coll, doc, @new);
            return Ok(JsonConvert.SerializeObject(@new, SerializerSettings));
        }
        
        [HttpDelete("remove/{coll}/{doc}")]
        public async Task<IActionResult> Remove(string coll, string doc)
        {
            var document = await Methods.Remove(coll, doc);
            if (!document) return BadRequest($"Unable to remove: {coll}/{doc}");
            
            return Ok(JsonConvert.SerializeObject("Success", SerializerSettings));
        }
        
        [HttpDelete("clear/{coll}")]
        public async Task<IActionResult> Clear(string coll)
        {
            var document = await Methods.Clear(coll);
            if (!document) return BadRequest($"Unable to clear: {coll}");
            
            return Ok(JsonConvert.SerializeObject("Success", SerializerSettings));
        }
    }
}