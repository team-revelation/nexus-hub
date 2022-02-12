using System;

namespace Types.Sockets
{
    public record SocketOptions
    {
        public SocketOptions(Guid userUuid, bool isTyping)
        {
            UserUuid = userUuid.ToString();
            IsTyping = isTyping;
        }
        
        public SocketOptions(Guid userUuid)
        {
            UserUuid = userUuid.ToString();
        }
        
        public SocketOptions(bool isTyping)
        {
            IsTyping = isTyping;
        }
        
        public SocketOptions () { }

        public string UserUuid { get; set; }
        public bool IsTyping { get; set; } = false;
    }
}