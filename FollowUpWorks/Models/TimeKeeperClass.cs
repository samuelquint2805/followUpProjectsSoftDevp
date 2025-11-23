using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class TimeKeeperClass : iID
    {
        public Guid idTimeKeeper { get; set; }
        public Guid Id { get; set; }
        public TimeSpan timeSpanKeeper { get; set; }
        public List<TimeSpan> laps { get; set; } = new List<TimeSpan>();
    }
}
