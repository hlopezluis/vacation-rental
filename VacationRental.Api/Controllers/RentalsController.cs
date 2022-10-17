using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;

        public RentalsController(IRentalsService service)
        {
            _rentalsService = service;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(String))]
        public IActionResult Get(int rentalId)
        {
            RentalViewModel rentalViewModel = _rentalsService.Get(rentalId);

            if (rentalViewModel == null)
                return NotFound("Rental not found");
            else
                return Ok(rentalViewModel);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResourceIdViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(String))]
        public IActionResult Post(RentalBindingModel model)
        {
            ResourceIdViewModel resourceIdViewModel = _rentalsService.AddRental(model);

            if (resourceIdViewModel == null)
                return BadRequest("Bad request");
            else
                return Ok(resourceIdViewModel);
        }
    }
}
