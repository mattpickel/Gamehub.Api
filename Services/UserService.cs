using Gamehub.Api.Models;
using Gamehub.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamehub.Api.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> UpdateUserAsync(int id, User updatedUserStats)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"No user found with ID {id}");
            }
            user.TotalGamesPlayed = updatedUserStats.TotalGamesPlayed;
            user.TotalGamesWon = updatedUserStats.TotalGamesWon;
            user.LongestStreak = updatedUserStats.LongestStreak;
            user.CurrentStreak = updatedUserStats.CurrentStreak;
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> CreateUserAsync(User newUser)
        {
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return newUser;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByClerkIdAsync(string clerkId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.ClerkId == clerkId);
        }

        public async Task<User> UpdateUserByClerkIdAsync(string clerkId, User updatedUserStats)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.ClerkId == clerkId);
            if (user == null)
            {
                throw new KeyNotFoundException($"No user found with ClerkId {clerkId}");
            }

            // Assuming you want to update these fields; add or remove fields as needed
            user.TotalGamesPlayed = updatedUserStats.TotalGamesPlayed;
            user.TotalGamesWon = updatedUserStats.TotalGamesWon;
            user.LongestStreak = updatedUserStats.LongestStreak;
            user.CurrentStreak = updatedUserStats.CurrentStreak;

            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}