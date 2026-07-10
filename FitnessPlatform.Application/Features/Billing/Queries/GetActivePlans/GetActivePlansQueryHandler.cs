using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Features.Billing.Queries.GetActivePlans
{
    // ۳. هندلر (پردازشگر)
    public class GetActivePlansQueryHandler : IRequestHandler<GetActivePlansQuery, IEnumerable<SubscriptionPlanDto>>
    {
        private readonly ISubscriptionPlanRepository _repository;

        public GetActivePlansQueryHandler(ISubscriptionPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SubscriptionPlanDto>> Handle(GetActivePlansQuery request, CancellationToken cancellationToken)
        {
            // خواندن موجودیت‌ها از دیتابیس
            var plans = await _repository.GetActivePlansAsync(cancellationToken);

            // تبدیل (Map) موجودیت‌های دامین به DTO
            return plans.Select(p => new SubscriptionPlanDto(
                p.Id,
                p.Title,
                p.Price,
                p.TotalSessions,
                p.ValidityDays
            ));
        }
    }
}
