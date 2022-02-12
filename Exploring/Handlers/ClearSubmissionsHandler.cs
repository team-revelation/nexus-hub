using System.Threading;
using System.Threading.Tasks;
using Contracts.Exploring;
using Exploring.Commands;
using MediatR;

namespace Exploring.Handlers
{
    public class ClearSubmissionsHandler : IRequestHandler<ClearSubmissionsCommand>
    {
        private readonly IExploreService _exploreService;
        public ClearSubmissionsHandler(IExploreService exploreService)
        {
            _exploreService = exploreService;
        }

        public async Task<Unit> Handle(ClearSubmissionsCommand request, CancellationToken cancellationToken)
        {
            await _exploreService.Clear();
            return Unit.Value;
        }
    }
}