using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Chats;
using Contracts.Redis;
using Contracts.Users;
using Newtonsoft.Json;
using Types.Sockets;
using Types.Users;

namespace Websockets.Services
{
    public class SocketService : ISocketService
    {
        private readonly Dictionary<WebSocket, SocketRequest> _sockets = new (); 

        private readonly IUserService _userService;
        private readonly IRedisService _redisService;

        public SocketService(IUserService userService, IRedisService redisService)
        {
            _userService = userService;
            _redisService = redisService;
        }

        /// <summary>
        /// Get a websocket with the given options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Dictionary<WebSocket, SocketRequest> Get(SocketOptions options)
        {
            return _sockets.Where(kvp => kvp.Value.Options.UserUuid == options.UserUuid).ToDictionary(s => s.Key, s=> s.Value);
        }
        
        /// <summary>
        /// Get all websockets.
        /// </summary>
        /// <returns></returns>
        public Dictionary<WebSocket, SocketRequest> All()
        {
            return _sockets;
        }
        
        /// <summary>
        ///     Listen to updates via a websocket.
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="bearer">Email of the user who sent the request.</param>
        /// <exception cref="EncoderFallbackException">A fallback occured while converting data to bytes.</exception>
        public async Task Listen(WebSocket webSocket, string bearer)
        {
            var buffer = new byte[1024 * 4];
            await Receive(webSocket, buffer);

            var request = JsonConvert.DeserializeObject<SocketRequest>(Encoding.UTF8.GetString(buffer));
            if (!await HandleRegister(bearer, request))
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Not allowed to listen to this user.", CancellationToken.None);
                return;
            }

            Clear(buffer);

            _sockets.Add(webSocket, request);
            await _redisService.Subscribe(request?.Options.UserUuid, async data => await Send(webSocket, data));
            while (webSocket.State == WebSocketState.Open)
            {
                await Receive(webSocket, buffer);
                var message = JsonConvert.DeserializeObject<SocketMessage>(Encoding.UTF8.GetString(buffer));
                if (message is not null)
                    await HandleMessage(webSocket, message);
            }

            _sockets.Remove(webSocket);
            await _redisService.Unsubscribe(request?.Options.UserUuid);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed connection.", CancellationToken.None);
        }

        private async Task Send(WebSocket webSocket, RedisData data)
        {
            if (webSocket.State != WebSocketState.Open) return;

            var (type, obj) = data;
            var payload = new SocketPayload(type, obj);
            await Send(webSocket, payload.ToString());
        }

        /// <summary>
        ///     Send data to a specific websocket.
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <exception cref="EncoderFallbackException">A fallback occured while converting data to bytes.</exception>
        public async Task Send(WebSocket webSocket, object data, string type)
        {
            if (webSocket.State != WebSocketState.Open) return;
            
            var payload = new SocketPayload(type, data);
            await Send(webSocket, payload.ToString());
        }

        /// <summary>
        ///     Send data to multiple websocket.
        /// </summary>
        /// <param name="webSockets"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <exception cref="EncoderFallbackException">A fallback occured while converting data to bytes.</exception>
        public async Task Send(List<WebSocket> webSockets, object data, string type)
        {
            var payload = new SocketPayload(type, data);
            foreach (var webSocket in webSockets)
                if (webSocket.State == WebSocketState.Open)
                    await Send(webSocket, payload.ToString());
        }

        private async Task HandleMessage(WebSocket webSocket, SocketMessage request)
        {
            switch (request.Type)
            {
                case SocketType.Typing:
                    if (Guid.TryParse(request.Options.UserUuid, out var guid))
                    {
                        // TODO: Implement Redis typing cache.
                    }
                    break;
                case SocketType.Ping:
                    await Send(webSocket, "{\"type\":\"pong\"}");
                    break;
            }
        }
        private async Task<bool> HandleRegister(string bearer, SocketRequest request)
        {
            User user = null;
            if (Guid.TryParse(request.Options.UserUuid, out var guid))
                user = await _userService.Get(guid);

            foreach (var type in request.Types)
            {
                switch (type)
                {
                    case SocketType.User:
                        return user is not null && user.Email == bearer;
                }
            }

            return true;
        }

        private async Task Send(WebSocket webSocket, string data)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }

        private async Task Receive(WebSocket webSocket, byte[] buffer)
        {
            Clear(buffer);
            await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        private void Clear(byte[] buffer)
        {
            Array.Clear(buffer, 0, buffer.Length);
        }

        public Task StartAsync(CancellationToken cancellationToken) { return Task.CompletedTask; }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var socket in _sockets)
            {
                await socket.Key.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Service restart.", cancellationToken);
                await socket.Key.CloseAsync(WebSocketCloseStatus.NormalClosure, "Service restart.", cancellationToken);
            }
            
            _sockets.Clear();
        }
    }
}