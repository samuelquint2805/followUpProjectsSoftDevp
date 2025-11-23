namespace FollowUpWorks.DTOs
{
    public class NoteClassDTO
    {
        public Guid idNotes { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
