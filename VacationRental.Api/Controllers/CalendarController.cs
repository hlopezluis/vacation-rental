using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingWithPreparationsViewModel> _bookings;

        public CalendarController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingWithPreparationsViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CalendarViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(String))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(String))]
        public IActionResult Get(int rentalId, DateTime start, int nights)
        {

            if (nights < 0)
                return BadRequest("Nights must be positive");

            if (!_rentals.ContainsKey(rentalId))
                return NotFound("Rental not found");

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

            return Ok(result);


        }
    }
}
