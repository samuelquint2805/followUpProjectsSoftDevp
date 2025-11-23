using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class ReservationClass : iID
    {
        public Guid idReservation { get; set; }
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        public int NumberOfGuests { get; set; } = 1;
        public string ContactInfo { get; set; } = string.Empty;
    }
}
