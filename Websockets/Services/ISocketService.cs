using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Types.Sockets;

namespace Websockets.Services
{
    public interface ISocketService : IHostedService
    {
        Task Listen(WebSocket webSocket, string bearer);
    }
}