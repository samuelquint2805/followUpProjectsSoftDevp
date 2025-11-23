using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class NoteClass : iID
    {
        public Guid idNotes { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;

    }
}
