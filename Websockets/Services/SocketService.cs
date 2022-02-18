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
            if (!await AuthorizeRegister(bearer, request))
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

        /// <summary>
        /// Send data through the websocket.
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="data"></param>
        private async Task Send(WebSocket webSocket, RedisData data)
        {
            if (webSocket.State != WebSocketState.Open) return;

            var (type, obj) = data;
            var payload = new SocketPayload(type, obj);
            await Send(webSocket, payload.ToString());
        }

        /// <summary>
        /// Handle websocket messages being sent through the websocket.
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="request"></param>
        private async Task HandleMessage(WebSocket webSocket, SocketMessage request)
        {
            switch (request.Type)
            {
                case SocketType.Typing:
                    break;
                case SocketType.Ping:
                    await Send(webSocket, "{\"type\":\"pong\"}");
                    break;
            }
        }
        
        /// <summary>
        /// Authorize the new websocket connection.
        /// </summary>
        /// <param name="bearer"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<bool> AuthorizeRegister(string bearer, SocketRequest request)
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

        /// <summary>
        /// Send a message through the websocket
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="data"></param>
        private async Task Send(WebSocket webSocket, string data)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
            else _sockets.Remove(webSocket);
        }

        /// <summary>
        /// Receive message from the websocket and put it in the buffer.
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="buffer"></param>
        private async Task Receive(WebSocket webSocket, byte[] buffer)
        {
            Clear(buffer);
            await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        /// <summary>
        /// Clear the current buffer.
        /// </summary>
        /// <param name="buffer"></param>
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