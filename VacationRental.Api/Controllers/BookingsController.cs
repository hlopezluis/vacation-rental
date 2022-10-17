using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {

        private readonly IBookingsService _bookingsService;

        public BookingsController(IBookingsService service)
        {
            _bookingsService = service;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookingViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(String))]
        public IActionResult Get(int bookingId)
        {
            BookingViewModel model = _bookingsService.Get(bookingId);
            if (model == null)
            {
                return NotFound("Booking not found");
            }
            else
            {
                return Ok(model);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResourceIdViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(String))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(String))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(String))]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed, Type = typeof(String))]
        public IActionResult Post(BookingBindingModel model)
        {
            ResourceIdViewModel resourceModel = _bookingsService.Add(model);
            if (resourceModel == null)
            {
                return ValidationProblem("Validation error", null, 412);
            }
            else
            {
                return Ok(resourceModel);
            }
        }
    }
}
