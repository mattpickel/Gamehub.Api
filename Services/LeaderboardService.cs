using Gamehub.Api.Models;
using Gamehub.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamehub.Api.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly AppDbContext _context;

        public LeaderboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LeaderboardDataDto> GetLeaderboardDataAsync()
        {
            var highestStreakLeaders = await _context.Users
                .OrderByDescending(u => u.LongestStreak)
                .Take(3)
                .Select(u => new LeaderboardEntryDto { Username = u.Username, Score = u.LongestStreak })
                .ToListAsync();

            var mostWinsLeaders = await _context.Users
                .OrderByDescending(u => u.TotalGamesWon)
                .Take(3)
                .Select(u => new LeaderboardEntryDto { Username = u.Username, Score = u.TotalGamesWon })
                .ToListAsync();

            // Construct the LeaderboardDataDto
            var leaderboardData = new LeaderboardDataDto
            {
                HighestStreakLeaders = highestStreakLeaders,
                MostWinsLeaders = mostWinsLeaders
                // Add other categories as necessary
            };

            return leaderboardData;
        }
    }
}