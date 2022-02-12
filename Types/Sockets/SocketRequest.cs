namespace Types.Sockets
{
    public record SocketRequest
    {
        public SocketType[] Types { get; init; }
        public SocketOptions Options { get; init; }
    }
}