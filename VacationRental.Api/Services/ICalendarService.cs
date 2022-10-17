using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public interface ICalendarService
    {
        public CalendarViewModel Get(int rentalId, DateTime start, int nights);
    }
}

