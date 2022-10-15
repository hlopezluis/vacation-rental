using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingWithPreparationsViewModel> _bookings;

        public BookingsController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingWithPreparationsViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(String))]
        public IActionResult Get(int bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
            {
                return NotFound("Booking not found");
            }
            else
            {
                return Ok(_bookings[bookingId]);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResourceIdViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(String))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(String))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(String))]
        public IActionResult Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
            {
                return BadRequest("Nigts must be positive");
            }
            else if (!_rentals.ContainsKey(model.RentalId))
            {
                return NotFound("Rental not found");
            }
            else if (areAllUnitsBooked(model))
            {
                return Conflict("All units of this rental are already booked");
            }
            else if (isThereAvailability(model))
            {
                var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

                _bookings.Add(key.Id, new BookingWithPreparationsViewModel
                {
                    Id = key.Id,
                    RentalId = model.RentalId,
                    Unit = getNextUnit(model.RentalId),
                    Bookings = getBookingData(model),
                    Preparations = getPreparationDays(model)
                });

                return Ok(key);
            }
            else
            {
                return Conflict("No availability for these dates");
            }
        }

        private bool areAllUnitsBooked(BookingBindingModel model)
        {
            bool result = true;

            for (int i = 1; i <= _rentals[model.RentalId].Units; i++)
            {
                int unitCount =
                (from booking in _bookings.Values
                 where (booking.Unit == i)
                 select booking).Count();

                if (unitCount == 0)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private bool isThereAvailability(BookingBindingModel model)
        {
            int rentalCount = _rentals[model.RentalId].Units;

            int preparationDays = (int)(_rentals[model.RentalId].PreparationTimeInDays == null ? 0 : _rentals[model.RentalId].PreparationTimeInDays);

            int bookingCount = (from booking in _bookings.Values
                                where (booking.Bookings.Start <= model.Start.Date && booking.Bookings.Start.AddDays(booking.Bookings.Nights + preparationDays) > model.Start.Date)
                                || (booking.Bookings.Start < model.Start.AddDays(model.Nights + preparationDays) && booking.Bookings.Start.AddDays(booking.Bookings.Nights + preparationDays) >= model.Start.AddDays(model.Nights + preparationDays))
                                || (booking.Bookings.Start > model.Start && booking.Bookings.Start.AddDays(booking.Bookings.Nights + preparationDays) < model.Start.AddDays(model.Nights + preparationDays))
                                select booking).Count();

            return (bookingCount < rentalCount);
        }

        private int getNextUnit(int rentalId)
        {
            if (_bookings == null)
            {
                return 1;
            }
            else
            {
                int unit = (from booking in _bookings.Values
                            where (booking.RentalId == rentalId)
                            select booking.Unit).FirstOrDefault();

                return unit = unit + 1;
            }
        }

        private BookingData getBookingData(BookingBindingModel model)
        {
            BookingData bookingData = new BookingData();
            bookingData.Nights = model.Nights;
            bookingData.Start = model.Start.Date;

            return bookingData;
        }

        private BookingData getPreparationDays(BookingBindingModel model)
        {
            BookingData preparationData = new BookingData();

            int? preaparationDays = (from rental in _rentals.Values
                                     where (rental.Id == model.RentalId)
                                     select rental.PreparationTimeInDays).FirstOrDefault();

            int days = (int)(preaparationDays == null ? 0 : preaparationDays);

            if (days > 0)
            {
                preparationData.Nights = days;
                preparationData.Start = model.Start.Date.AddDays(model.Nights);
            }

            return preparationData;
        }

    }
}
