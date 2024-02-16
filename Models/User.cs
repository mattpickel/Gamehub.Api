namespace Gamehub.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        // Username is required for login or identification
        public string Username { get; set; } = string.Empty;

        // Names not required
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string ClerkId { get; set; } = string.Empty;
        // Stats start at 0 
        public int TotalGamesPlayed { get; set; } = 0;
        public int TotalGamesWon { get; set; } = 0;
        public int LongestStreak { get; set; } = 0;
        public int CurrentStreak { get; set; } = 0;
    }
}