using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class EventClass : iID
    {
      

        public Guid Id { get; set; }

        public Guid idEvent { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? EventDate { get; set; } = DateTime.Now;
        public string? Location { get; set; } = string.Empty;
        
    }
}
