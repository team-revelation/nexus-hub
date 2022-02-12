using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Types.Users;

namespace Contracts.Users
{
    public interface IUserService
    {
        Task<User> Create(User user);
        Task<IEnumerable<User>> All();
        Task<IEnumerable<User>> Query(IEnumerable<Guid> users);
        Task<User> Get(Guid uuid);
        Task<User> Get(string email);
        Task<User> Update(Guid uuid, User user);
        Task<IEnumerable<User>> Update(Dictionary<Guid, User> users);
        Task Delete(Guid uuid);
        Task Clear();
    }
}