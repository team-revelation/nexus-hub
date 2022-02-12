using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Types.Exploring;
using Types.Users;

namespace Contracts.Exploring
{
    public interface IExploreService
    {
        Task<Submission> Create(User user, Position position);
        
        Task<IEnumerable<Submission>> All();
        Task<Submission> Get(Guid uuid);
        
        Task<Submission> Update(User user, Position position);
        
        Task Delete(Guid uuid);
        Task Clear();
    }
}