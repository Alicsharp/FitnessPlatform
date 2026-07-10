using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Commands.Purchase
{
    // کاربر فقط میگوید من هستم، این طرح را می‌خواهم و اینقدر پول دادم
    public record PurchaseSubscriptionCommand(
        Guid UserId,
        Guid PlanId,
        decimal AmountPaid) : IRequest<Guid>;

    public class PurchaseSubscriptionCommandHandler : IRequestHandler<PurchaseSubscriptionCommand, Guid>
    {
        private readonly IUserSubscriptionRepository _subscriptionRepository;
        private readonly ISubscriptionPlanRepository _planRepository;

        // اینجا به هر دو مخزن نیاز داریم
        public PurchaseSubscriptionCommandHandler(
            IUserSubscriptionRepository subscriptionRepository,
            ISubscriptionPlanRepository planRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _planRepository = planRepository;
        }

        public async Task<Guid> Handle(PurchaseSubscriptionCommand request, CancellationToken cancellationToken)
        {
            // ۱. دریافت طرح اشتراک از دیتابیس برای واکشی اطلاعات جلسات و روزها
            var plan = await _planRepository.GetByIdAsync(request.PlanId, cancellationToken);

            if (plan == null || !plan.IsActive)
            {
                throw new InvalidOperationException("طرح انتخابی معتبر نیست یا غیرفعال شده است.");
            }

            // قانون بیزینسی: چک کردن مبلغ پرداختی با قیمت اصلی طرح
            if (request.AmountPaid < plan.Price)
            {
                throw new InvalidOperationException("مبلغ پرداختی کمتر از قیمت طرح است.");
            }

            // ۲. ساخت اشتراک کاربر بر اساس اطلاعاتی که از Plan گرفتیم
            var subscription = UserSubscription.Create(
                request.UserId,
                plan.Id,
                plan.TotalSessions,
                plan.ValidityDays,
                request.AmountPaid);

            // ۳. ذخیره فاکتور در دیتابیس
            await _subscriptionRepository.AddAsync(subscription, cancellationToken);

            // ۴. برگرداندن شناسه اشتراک خریداری شده
            return subscription.Id;
        }
    }
}
