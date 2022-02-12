using System.Threading;
using System.Threading.Tasks;
using Contracts.Exploring;
using Exploring.Commands;
using MediatR;

namespace Exploring.Handlers
{
    public class RemoveSubmissionHandler: IRequestHandler<RemoveSubmissionCommand>
    {
        private readonly IExploreService _exploreService;
        
        public RemoveSubmissionHandler(IExploreService exploreService)
        {
            _exploreService = exploreService;
        }

        public async Task<Unit> Handle(RemoveSubmissionCommand request, CancellationToken cancellationToken)
        {
            await _exploreService.Delete(request.Uuid);
            return Unit.Value;
        }
    }
}