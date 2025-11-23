using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class MemoryGameClass : iID
    {
        public Guid idGame { get; set; } = Guid.NewGuid();

        public Guid Id
        {
            get => idGame;
            set => idGame = value;
        }
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public DateTime DatePlayed { get; set; } = DateTime.Now;
    }
}
