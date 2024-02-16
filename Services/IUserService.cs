using Gamehub.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamehub.Api.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByClerkIdAsync(string clerkId);
        Task<User> UpdateUserAsync(int id, User updatedUserStats);
        Task<User> CreateUserAsync(User newUser);
        Task<User> UpdateUserByClerkIdAsync(string clerkId, User updatedUserStats);
        Task<List<User>> GetAllUsersAsync();
    }
}