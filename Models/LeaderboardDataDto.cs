using Gamehub.Api.Models;

namespace Gamehub.Api.Models
{
    public class LeaderboardDataDto
    {
        public List<LeaderboardEntryDto> HighestStreakLeaders { get; set; } = new List<LeaderboardEntryDto>();
        public List<LeaderboardEntryDto> MostWinsLeaders { get; set; } = new List<LeaderboardEntryDto>();
    }
}