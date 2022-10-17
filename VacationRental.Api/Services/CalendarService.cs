using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class CalendarService: ICalendarService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingWithPreparationsViewModel> _bookings;

        public CalendarService(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingWithPreparationsViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0 || !_rentals.ContainsKey(rentalId))
                return null;
            else
            {
                var result = new CalendarViewModel
                {
                    RentalId = rentalId,
                    Dates = new List<CalendarDateViewModel>()
                };
                for (var i = 0; i < nights; i++)
                {
                    var date = new CalendarDateViewModel
                    {
                        Date = start.Date.AddDays(i),
                        Bookings = new List<CalendarBookingViewModel>(),
                        Preparations = new List<CalendarPreparationTimeViewModel>()
                    };

                    foreach (var booking in _bookings.Values)
                    {
                        if (booking.RentalId == rentalId
                            && booking.Bookings.Start <= date.Date && booking.Bookings.Start.AddDays(booking.Bookings.Nights) > date.Date)
                        {
                            date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                        }
                        if (booking.RentalId == rentalId
                           && booking.Preparations.Start <= date.Date && booking.Preparations.Start.AddDays(booking.Preparations.Nights) > date.Date)
                        {
                            date.Preparations.Add(new CalendarPreparationTimeViewModel { Id = booking.Id });
                        }
                    }

                    result.Dates.Add(date);
                }

                return result;
            }
        }
    }
}

