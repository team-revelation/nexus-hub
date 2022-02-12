using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Database;
using Types.Chats;

namespace Contracts.Chats
{
    public class ChatService : IChatService
    {
        private readonly IDatabaseService _databaseService;
        
        public ChatService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        ///     Retrieve all the chats from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Chat>> All()
        {
            return await _databaseService.All<Chat>("chats");
        }
        
        /// <summary>
        ///     Retrieve all chats with query. 
        /// </summary>
        /// <param name="chats"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Chat>> Query(IEnumerable<Guid> chats)
        {
            return await _databaseService.Query<Chat>("chats", chats.Select(c => c.ToString("D")).ToList());
        }

        /// <summary>
        ///     Retrieve all the chats that the member is part of.
        /// </summary>
        /// <param name="memberUuid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Chat>> All(Guid memberUuid)
        {
            var chats = await _databaseService.All<Chat>("chats");
            return chats.Where(chat => chat.Members.Any(member => member.Uuid == memberUuid));
        }
        
        /// <summary>
        ///     Retrieve a chat from the database.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The chat does not exist.</exception>
        public async Task<Chat> Get(Guid uuid)
        {
            var result =  await _databaseService.Get<Chat>("chats", uuid.ToString("D"));
            return result ?? throw new ArgumentException("This chat does not exist.");
        }

        /// <summary>
        ///     Add a new chat.
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">The server failed to add the chat.</exception>
        public async Task<Chat> Create(Chat chat)
        {
            if (chat.Uuid == Guid.Empty) chat.Uuid = Guid.NewGuid();
            await _databaseService.Create("chats", chat.Uuid.ToString("D"), chat, typeof(Chat));
            return chat;
        }

        /// <summary>
        ///     Update a chat with new data.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The chat does not exist.</exception>
        /// <exception cref="ApplicationException">The server failed to update the chat.</exception>
        public async Task<Chat> Update(Guid uuid, Chat chat)
        {
            await _databaseService.Update("chats", chat.Uuid.ToString("D"), chat, typeof(Chat));
            return chat;
        }
        
        /// <summary>
        ///     Update a collection of existing chats.
        /// </summary>
        /// <param name="chats"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        public async Task<IEnumerable<Chat>> Update(Dictionary<Guid, Chat> chats)
        {
            List<Chat> result = new();
            foreach (var (uuid, chat) in chats)
                result.Add(await Update(uuid, chat));

            return result;
        }

        /// <summary>
        ///     Delete an already existing chat from the database.
        /// </summary>
        /// <param name="uuid"></param>
        /// <exception cref="ApplicationException">The server failed to remove the chat.</exception>
        public async Task Delete(Guid uuid)
        {
            await _databaseService.Delete("chats", uuid.ToString("D"));
        }

        /// <summary>
        ///     Remove all chats.
        /// </summary>
        public async Task Clear()
        {
            await _databaseService.Clear("chats");
        }
    }
}