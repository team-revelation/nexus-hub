using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Database;
using Google.Cloud.Firestore;
using Types.Exploring;
using Types.Users;

namespace Contracts.Exploring
{
    public class ExploreService : IExploreService
    {
        private readonly IDatabaseService _databaseService;
        
        public ExploreService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        ///     Retrieve all the submissions from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Submission>> All()
        {
            return await _databaseService.All<Submission>("submissions");
        }
        
        /// <summary>
        ///     Retrieve a submission from the database by uuid.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        public async Task<Submission> Get(Guid uuid)
        {
            var result =  await _databaseService.Get<Submission>("submissions", uuid.ToString("D"));
            return result ?? throw new ArgumentException("This user does not exist.");
        }

        /// <summary>
        ///     Add a new submission to the database.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">The server failed to add the user.</exception>
        public async Task<Submission> Create(User user, Position position)
        {
            if (user.Uuid == Guid.Empty) user.Uuid = Guid.NewGuid();
            var submission = new Submission(user, position);
            await _databaseService.Create("submissions", user.Uuid.ToString("D"), submission, typeof(Submission));
            return submission;
        }

        /// <summary>
        ///     Update an already existing submission.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        /// <exception cref="ApplicationException">The server failed to update the user.</exception>
        public async Task<Submission> Update(User user, Position position)
        {
            var existingSubmission = await Get(user.Uuid);
            if (existingSubmission is null)
                throw new ArgumentException("This submission does not exist.");

            existingSubmission = existingSubmission with
            {
                Username = user.Username ?? existingSubmission.Username,
                Avatar = user.Avatar ?? existingSubmission.Avatar,
                Description = user.Description ?? existingSubmission.Description,
                Interests = user.Interests ?? existingSubmission.Interests,
                Position = position
            };

            await _databaseService.Update("submissions", user.Uuid.ToString("D"), existingSubmission, typeof(Submission));
            return existingSubmission;
        }

        /// <summary>
        ///     Remove a submission from the database.
        /// </summary>
        /// <param name="uuid"></param>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        /// <exception cref="ApplicationException">The server failed to remove the user.</exception>
        public async Task Delete(Guid uuid)
        {
            await _databaseService.Delete("submissions", uuid.ToString("D"));
        }
        
        /// <summary>
        ///     Remove a submission from the database.
        /// </summary>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        /// <exception cref="ApplicationException">The server failed to remove the user.</exception>
        public async Task Clear()
        {
            await _databaseService.Clear("submissions");
        }
    }
}