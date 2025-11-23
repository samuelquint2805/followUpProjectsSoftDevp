namespace FollowUpWorks.DTOs
{
    public class TimeKeeperClassDTO
    {
        public Guid idTimeKeeper { get; set; }
        public TimeSpan timeSpanKeeper { get; set; }
        public List<TimeSpan> laps { get; set; } = new List<TimeSpan>();
    }
}
