using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Booking.Queries.GetAvailableClasses
{
    public record GroupClassDto(Guid Id, string Title, DateTime StartTime, int MaxCapacity, int EnrolledCount);
}
