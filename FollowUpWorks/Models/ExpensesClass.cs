using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class ExpensesClass : iID
    {
        public Guid idExpenses { get; set; } 
        public Guid Id {  get; set; }
        public DateTime Date { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
       
    }
}
