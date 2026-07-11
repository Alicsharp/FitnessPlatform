using MediatR;

namespace FitnessPlatform.Application.Features.Booking.Queries.GetAvailableClasses
{
    public record GetAvailableClassesQuery() : IRequest<IEnumerable<GroupClassDto>>;
}
