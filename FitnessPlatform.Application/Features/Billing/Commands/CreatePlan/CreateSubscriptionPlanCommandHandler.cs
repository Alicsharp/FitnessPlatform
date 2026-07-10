using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Features.Billing.Commands.CreatePlan
{
    public class CreateSubscriptionPlanCommandHandler : IRequestHandler<CreateSubscriptionPlanCommand, Guid>
    {
        private readonly ISubscriptionPlanRepository _repository;

        // تزریق وابستگی مخزن طرح‌های اشتراک
        public CreateSubscriptionPlanCommandHandler(ISubscriptionPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            // ۱. ساخت موجودیت اصلی با استفاده از متد امنِ Factory در Domain
            var plan = SubscriptionPlan.Create(
                request.Title,
                request.Price,
                request.TotalSessions,
                request.ValidityDays);

            // ۲. ذخیره در دیتابیس (از طریق قرارداد/اینترفیس)
            await _repository.AddAsync(plan, cancellationToken);

            // ۳. برگرداندن شناسه طرح ساخته شده
            return plan.Id;
        }
    }
}

