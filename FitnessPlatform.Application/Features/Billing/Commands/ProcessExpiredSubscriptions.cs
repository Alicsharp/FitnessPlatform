using FitnessPlatform.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Commands
{
    // این کامند هیچ ورودی خاصی ندارد، چون خودش بر اساس زمان حال تصمیم می‌گیرد
    public record ProcessExpiredSubscriptionsCommand() : IRequest<int>;

    public class ProcessExpiredSubscriptionsCommandHandler : IRequestHandler<ProcessExpiredSubscriptionsCommand, int>
    {
        private readonly IUserSubscriptionRepository _repository;
        // فرض می‌کنیم یک IEventPublisher داریم که رویدادها را به RabbitMQ می‌فرستد
        // (بعداً آن را در زیرساخت پیاده می‌کنیم)

        public ProcessExpiredSubscriptionsCommandHandler(IUserSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(ProcessExpiredSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            // ۱. پیدا کردن اشتراک‌هایی که تاریخشان گذشته اما هنوز جلساتشان صفر نشده است
            // (باید متد GetExpiredSubscriptionsAsync را به ریپازیتوری اضافه کنی)
            var expiredSubscriptions = await _repository.GetExpiredSubscriptionsAsync(cancellationToken);

            int processedCount = 0;

            // ۲. اجرای لاجیک دامین روی تک‌تک آن‌ها
            foreach (var subscription in expiredSubscriptions)
            {
                subscription.Expire();

                await _repository.UpdateAsync(subscription, cancellationToken);

                // ⚡️ اینجا در یک سیستم واقعی، Domain Events استخراج شده 
                // و توسط IMessageBus به صف RabbitMQ فرستاده می‌شوند.

                processedCount++;
            }

            // برگرداندن تعداد اشتراک‌های منقضی شده برای لاگ‌گیری
            return processedCount;
        }
    }
}
