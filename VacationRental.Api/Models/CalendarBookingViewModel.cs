namespace VacationRental.Api.Models
{
    public class CalendarCommonViewModel
    {
        public int Id { get; set; }
    }

    public class CalendarBookingViewModel : CalendarCommonViewModel
    {
        public int Unit { get; set; }
    }

    public class CalendarPreparationTimeViewModel : CalendarCommonViewModel
    {

    }
}
