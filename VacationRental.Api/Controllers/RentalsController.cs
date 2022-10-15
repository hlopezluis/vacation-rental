using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public RentalsController(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(String))]
        public IActionResult Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
            {
                return NotFound("Rental not found");
            }
            else
            {
                return Ok(_rentals[rentalId]);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResourceIdViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(String))]
        public IActionResult Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return Ok(key);
        }
    }
}
