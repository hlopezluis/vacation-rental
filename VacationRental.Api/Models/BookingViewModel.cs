using System;

namespace VacationRental.Api.Models
{
    public class BookingData
    {
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }

    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int Unit { get; set; }
        public BookingData Bookings { get; set; }
    }

    public class BookingWithPreparationsViewModel : BookingViewModel
    {
        public BookingData Preparations { get; set; }
    }
}
