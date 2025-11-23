namespace FollowUpWorks.DTOs
{
    public class MemoryGameClassDTO
    {
        public Guid idGame { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public DateTime DatePlayed { get; set; } = DateTime.Now;
    }
}
