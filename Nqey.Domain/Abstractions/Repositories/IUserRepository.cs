using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User> DeleteUserAsync(int id);
        Task<int?> GetUserIdByUserNameAsync(string userName);

        Task<User> ActivateUser(int userId);
        Task<User> BlockUser(int userId);
    }
}
