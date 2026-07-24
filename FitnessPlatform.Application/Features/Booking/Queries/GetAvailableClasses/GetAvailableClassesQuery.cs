using FitnessPlatform.Application.Common;
using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Features.Booking.Queries.GetAvailableClasses
{
    public record GetAvailableClassesQuery : IRequest<List<GroupClassDto>>, ICacheableQuery
    {
        // کلید یکتا در رِدیس
        public string CacheKey => "available_group_classes";

        // مدت زمان اعتبار کش (مثلاً ۵ دقیقه)
        public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
    }
}
