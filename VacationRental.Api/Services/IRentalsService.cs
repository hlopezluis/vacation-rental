using System;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IRentalsService
    {
        RentalViewModel Get(int rentalId);
        ResourceIdViewModel AddRental(RentalBindingModel model);
        //RentalViewModel UpdateRental(int rentalId, RentalBindingModel model);
    }
}

