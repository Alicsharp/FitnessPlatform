using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Booking.Commands.CreateGroupClass
{
    public record CreateGroupClassCommand(string Title, DateTime StartTime, int MaxCapacity) : IRequest<Guid>;
}
