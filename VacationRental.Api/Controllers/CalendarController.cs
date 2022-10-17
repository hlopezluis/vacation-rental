using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService service)
        {
            _calendarService = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CalendarViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(String))]
        public IActionResult Get(int rentalId, DateTime start, int nights)
        {
            CalendarViewModel calendarViewModel = _calendarService.Get(rentalId, start, nights);

            if (calendarViewModel == null)
                return BadRequest("Bad request");
            else
                return Ok(calendarViewModel);

        }
    }
}
