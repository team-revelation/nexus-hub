using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Database;
using Types.Users;

namespace Contracts.Users
{
    public class UserService : IUserService
    {
        private readonly IDatabaseService _databaseService;
        
        public UserService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        ///     Retrieve all the users from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> All()
        {
            return await _databaseService.All<User>("users");
        }
        
        /// <summary>
        ///     Retrieve all chats with query.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<IEnumerable<User>> Query(IEnumerable<Guid> users)
        {
            return await _databaseService.Query<User>("users", users.Select(c => c.ToString("D")).ToList());
        }

        /// <summary>
        ///     Retrieve a user from the database by uuid.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        public async Task<User> Get(Guid uuid)
        {
            var result =  await _databaseService.Get<User>("users", uuid.ToString("D"));
            return result ?? throw new ArgumentException("This user does not exist.");
        }

        /// <summary>
        ///     Retrieve a user from the database by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        public async Task<User> Get(string email)
        {
            var users = await _databaseService.All<User>("users");
            var user = users.FirstOrDefault(user => user.Email == email);
            return user ?? throw new ArgumentException("This user does not exist.");
        }

        /// <summary>
        ///     Add a new user to the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">The server failed to add the user.</exception>
        public async Task<User> Create(User user)
        {
            if (user.Uuid == Guid.Empty) user.Uuid = Guid.NewGuid();
            await _databaseService.Create("users", user.Uuid.ToString("D"), user);
            return user;
        }

        /// <summary>
        ///     Update an already existing user.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        /// <exception cref="ApplicationException">The server failed to update the user.</exception>
        public async Task<User> Update(Guid uuid, User user)
        {
            var existingUser = await Get(uuid);
            if (existingUser is null)
                throw new ArgumentException("This user does not exist.");

            user.Uuid = uuid;
            existingUser = existingUser with
            {
                Uuid = user.Uuid,
                Username = user.Username ?? existingUser.Username,
                Email = user.Email ?? existingUser.Email,
                Avatar = user.Avatar ?? existingUser.Avatar,
                Description = user.Description ?? existingUser.Description,
                Interests = user.Interests ?? existingUser.Interests,
                Devices = user.Devices ?? existingUser.Devices,
                Friends = user.Friends ?? existingUser.Friends
            };

            await _databaseService.Update("users", uuid.ToString("D"), existingUser);
            return existingUser;
        }

        /// <summary>
        ///     Update a collection of existing users.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        public async Task<IEnumerable<User>> Update(Dictionary<Guid, User> users)
        {
            List<User> result = new();
            foreach (var (uuid, user) in users)
                result.Add(await Update(uuid, user));

            return result;
        }

        /// <summary>
        ///     Remove a user from the database.
        /// </summary>
        /// <param name="uuid"></param>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        /// <exception cref="ApplicationException">The server failed to remove the user.</exception>
        public async Task Delete(Guid uuid)
        {
            await _databaseService.Delete("users", uuid.ToString("D"));
        }

        /// <summary>
        ///     Delete every user.
        /// </summary>
        /// <exception cref="ArgumentException">The user does not exist.</exception>
        /// <exception cref="ApplicationException">The server failed to remove a user.</exception>
        public async Task Clear()
        {
            await _databaseService.Clear("users");
        }
    }
}