using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Types.Chats;

namespace Contracts.Chats
{
    public interface IChatService
    {
        Task<IEnumerable<Chat>> All();
        Task<IEnumerable<Chat>> All(Guid memberUuid);
        Task<IEnumerable<Chat>> Query(IEnumerable<Guid> chats);
        Task<Chat> Get(Guid uuid);

        Task<Chat> Create(Chat chat);
        Task<Chat> Update(Guid uuid, Chat chat);
        Task<IEnumerable<Chat>> Update(Dictionary<Guid, Chat> chats);
        Task Delete(Guid uuid);
        Task Clear();
    }
}