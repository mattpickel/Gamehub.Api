using Gamehub.Api.Models;

namespace Gamehub.Api.Services
{
    public interface ILeaderboardService
    {
        Task<LeaderboardDataDto> GetLeaderboardDataAsync();
    }
}