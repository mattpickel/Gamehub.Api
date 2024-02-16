namespace Gamehub.Api.Models
{
    public class LeaderboardEntryDto
    {
        public string Username { get; set; }
        public int Score { get; set; } // Streak, wins, etc. depending on leaderboard section
    }
}