using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Exploring;
using Contracts.Users;
using Exploring.Commands;
using MediatR;
using Types.Chats;
using Types.Exploring;
using Types.Users;

namespace Exploring.Handlers
{
    public class CreateSubmissionHandler : IRequestHandler<CreateSubmissionCommand, Submission>
    {
        private readonly IExploreService _exploreService;
        
        public CreateSubmissionHandler(IExploreService exploreService)
        {
            _exploreService = exploreService;
        }

        public async Task<Submission> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
        {
            return await _exploreService.Create(request.User, request.Position);
        }
    }
}