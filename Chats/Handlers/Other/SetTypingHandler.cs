using System.Threading;
using System.Threading.Tasks;
using Chats.Commands.Other;
using Types.Chats;
using Contracts.Memorystore;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Chats.Handlers.Other
{
    public class SetTypingHandler : IRequestHandler<SetTypingCommand, Status>
    {
        private readonly IMemorystoreService _cacheService;

        public SetTypingHandler(IMemorystoreService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<Status> Handle(SetTypingCommand request, CancellationToken cancellationToken)
        {
            var uuid = request.Chat.Uuid.ToString("D");
            var status = await _cacheService.Get<Status>("typing", uuid, async key =>
            {
                var typing = request.Chat.Members.Select(m => new Typing(m.Uuid, m.Uuid == request.UserUuid ? request.IsTyping : false)).ToList();
                var s = new Status(request.Chat.Uuid, typing);

                await _cacheService.Create("typing", key, s);
                return s;
            });

            var index = status.Typing.FindIndex(t => t.UserUuid == request.UserUuid);

            if (index == -1) status.Typing.Add(new Typing(request.UserUuid, request.IsTyping));
            else status.Typing[index] = status.Typing[index] with { IsTyping = request.IsTyping };

            await _cacheService.Update("typing", uuid, status);
            return status;
        }
    }
}