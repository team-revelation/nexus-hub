using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Types.Sockets;

namespace Websockets.Services
{
    public interface ISocketService : IHostedService
    {
        Dictionary<WebSocket, SocketRequest> Get(SocketOptions options);
        Dictionary<WebSocket, SocketRequest> All();
        Task Listen(WebSocket webSocket, string bearer);
        Task Send(WebSocket webSocket, object data, string type);
        Task Send(List<WebSocket> webSockets, object data, string type);
    }
}