using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.DTOs
{
    public class EventClassDTO 
    {
        public Guid idEvent { get; set; }
        
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; } = DateTime.Now;
        public string? Location { get; set; } = string.Empty;
    }
}
