namespace FollowUpWorks.DTOs
{
    public class ReservationClassDTO
    {
        public Guid idReservation { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        public int NumberOfGuests { get; set; } = 1;
        public string ContactInfo { get; set; } = string.Empty;
    }
}
