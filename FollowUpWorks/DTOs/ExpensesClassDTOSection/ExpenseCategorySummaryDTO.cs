namespace FollowUpWorks.DTOs.ExpensesClassDTOSection
{
    public class ExpenseCategorySummaryDTO
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Count { get; set; }
    }
}
