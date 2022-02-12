using System.Threading;
using System.Threading.Tasks;
using Contracts.Exploring;
using Contracts.Users;
using Exploring.Commands;
using MediatR;
using Types.Chats;
using Types.Exploring;

namespace Exploring.Handlers
{
    public class UpdateSubmissionHandler : IRequestHandler<UpdateSubmissionCommand, Submission>
    {
        private readonly IExploreService _exploreService;
        private readonly IUserService _userService;
        
        public UpdateSubmissionHandler(IExploreService exploreService, IUserService userService)
        {
            _exploreService = exploreService;
            _userService = userService;
        }
        
        public async Task<Submission> Handle(UpdateSubmissionCommand request, CancellationToken cancellationToken)
        {
            return await _exploreService.Update(request.User, request.Position);
        }
    }
}