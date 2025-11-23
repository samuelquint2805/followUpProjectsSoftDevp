using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class TipClass : iID
    {
        public Guid IdTips { get; set; }
        public Guid Id { get; set; }
        public decimal TipAmount { get; set; } = 0;
        public int tipPercentage { get; set; } = 0;
        public decimal TipResult { get; set; } = 0;
    }
}
