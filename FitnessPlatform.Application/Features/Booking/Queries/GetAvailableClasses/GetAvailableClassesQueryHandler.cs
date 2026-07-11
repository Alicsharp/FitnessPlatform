using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Features.Booking.Queries.GetAvailableClasses
{
    public class GetAvailableClassesQueryHandler : IRequestHandler<GetAvailableClassesQuery, IEnumerable<GroupClassDto>>
    {
        private readonly IGroupClassRepository _repository;

        public GetAvailableClassesQueryHandler(IGroupClassRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GroupClassDto>> Handle(GetAvailableClassesQuery request, CancellationToken cancellationToken)
        {
            var classes = await _repository.GetAvailableClassesAsync(cancellationToken);

            return classes.Select(c => new GroupClassDto(
                c.Id,
                c.Title,
                c.StartTime,
                c.MaxCapacity,
                c.EnrolledMembers.Count // تعداد افراد ثبت‌نام شده
            ));
        }
    }
}
