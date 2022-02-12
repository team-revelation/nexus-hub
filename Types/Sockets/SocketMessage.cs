namespace Types.Sockets
{
    public record SocketMessage
    {
        public SocketType Type { get; init; }
        public SocketOptions Options { get; init; }

        public static explicit operator SocketRequest(SocketMessage message)
        {
            return new SocketRequest
            {
                Types = new[] { message.Type },
                Options = message.Options,
            };
        }
    }
}