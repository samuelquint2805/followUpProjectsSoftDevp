using FollowUpWorks.Models;

namespace FollowUpWorks.services.Abstractions
{
    public interface IJSONReservation
    {
        List<ReservationClass> GetAll();
        void SaveAll(List<ReservationClass> reservation);
        void UpdateReservation(ReservationClass updatedReservation);
        void DeleteReservation(Guid id);
    }
}
