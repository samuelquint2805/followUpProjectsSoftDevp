namespace FollowUpWorks.DTOs
{
    public class TipClassDTO
    {
        public Guid IdTips { get; set; }
        public decimal TipAmount { get; set; } = 0;
        public int tipPercentage { get; set; } = 0;
        public decimal TipResult { get; set; } = 0;
    }
}
