using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Exploring;
using Contracts.Users;
using Exploring.Queries;
using MediatR;
using Types.Chats;
using Types.Exploring;

namespace Exploring.Handlers
{
    public class GetSubmissionsHandler : IRequestHandler<GetSubmissionsQuery, IEnumerable<Submission>>
    {
        private readonly IExploreService _exploreService;
        private readonly IUserService _userService;

        public GetSubmissionsHandler(IExploreService exploreService, IUserService userService)
        {
            _exploreService = exploreService;
            _userService = userService;
        }
        
        public async Task<IEnumerable<Submission>> Handle(GetSubmissionsQuery request, CancellationToken cancellationToken)
        {
            return await _exploreService.All();
        }
    }
}