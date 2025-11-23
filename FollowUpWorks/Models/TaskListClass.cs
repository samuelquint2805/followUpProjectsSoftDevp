using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class TaskListClass : iID
    {
        public Guid idTask { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public bool? IsCompleted { get; set; } = false;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
