using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface IBookingsService
    {
        public BookingViewModel Get(int bookingId);
        public ResourceIdViewModel Add(BookingBindingModel model);
    }
}

