using System.Threading;
using System.Threading.Tasks;
using Contracts.Exploring;
using Exploring.Queries;
using MediatR;
using Types.Chats;
using Types.Exploring;

namespace Exploring.Handlers
{
    public class GetSubmissionHandler : IRequestHandler<GetSubmissionQuery, Submission>
    {
        private readonly IExploreService _exploreService;

        public GetSubmissionHandler(IExploreService exploreService)
        {
            _exploreService = exploreService;
        }

        public async Task<Submission> Handle(GetSubmissionQuery request, CancellationToken cancellationToken)
        {
            return await _exploreService.Get(request.Uuid);
        }
    }
}