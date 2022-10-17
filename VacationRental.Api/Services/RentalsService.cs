using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class RentalsService: IRentalsService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public RentalsService(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        public ResourceIdViewModel AddRental(RentalBindingModel model)
        {
            if (rentalValidation(model))
                return null;
            else
            {
                var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

                _rentals.Add(key.Id, new RentalViewModel
                {
                    Id = key.Id,
                    Units = model.Units,
                    PreparationTimeInDays = model.PreparationTimeInDays
                });

                return key;
            }
        }
    

        public RentalViewModel Get(int rentalId)
        {
            if (!_rentals.ContainsKey(rentalId))
            {
                return null;
            }
            else
            {
                return _rentals[rentalId];
            }
        }

        private bool rentalValidation(RentalBindingModel model)
        {
            return (model.Units < 1 || model.PreparationTimeInDays != null && model.PreparationTimeInDays < 0);
        }
    }
}

