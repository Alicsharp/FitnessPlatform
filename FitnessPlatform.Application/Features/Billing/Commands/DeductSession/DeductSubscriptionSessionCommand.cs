using FitnessPlatform.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.Billing.Commands.DeductSession
{
    // فقط شناسه کاربری که تمرینش تمام شده را می‌گیریم
    public record DeductSubscriptionSessionCommand(Guid UserId) : IRequest<bool>;

    public class DeductSubscriptionSessionCommandHandler : IRequestHandler<DeductSubscriptionSessionCommand, bool>
    {
        private readonly IUserSubscriptionRepository _repository;

        public DeductSubscriptionSessionCommandHandler(IUserSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeductSubscriptionSessionCommand request, CancellationToken cancellationToken)
        {
            // ۱. پیدا کردن فاکتور/اشتراک فعال کاربر
            var subscription = await _repository.GetActiveSubscriptionByUserIdAsync(request.UserId, cancellationToken);

            if (subscription == null)
            {
                throw new InvalidOperationException("شما هیچ اشتراک فعالی برای کسر جلسه ندارید.");
            }

            // ۲. صدا زدن متد کپسوله‌شده در لایه Domain (قوانین و تاریخ‌ها خودکار چک می‌شوند)
            subscription.UseSession();

            // ۳. بروزرسانی در دیتابیس
            // نکته: اگر از EF Core استفاده می‌کنیم و Track شده باشد، فقط SaveChanges نیاز است
            // اما برای تمیزی در معماری، متد Update را صدا می‌زنیم
            await _repository.UpdateAsync(subscription, cancellationToken);

            return true;
        }
    }
}
