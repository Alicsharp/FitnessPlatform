using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Features.Billing.Queries.GetActiveUserSubscription
{
    // ۳. هندلر (پردازشگر)
    public class GetActiveUserSubscriptionQueryHandler : IRequestHandler<GetActiveUserSubscriptionQuery, UserSubscriptionDto?>
    {
        private readonly IUserSubscriptionRepository _repository;

        public GetActiveUserSubscriptionQueryHandler(IUserSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserSubscriptionDto?> Handle(GetActiveUserSubscriptionQuery request, CancellationToken cancellationToken)
        {
            // صدا زدن متد مخزن که قبلاً در اینترفیس تعریف کرده بودیم
            var subscription = await _repository.GetActiveSubscriptionByUserIdAsync(request.UserId, cancellationToken);

            // اگر کاربر هیچ اشتراکی نداشت (یا منقضی شده بود و رکورد فعالی پیدا نشد)
            if (subscription == null)
                return null;

            // مپ کردن اطلاعات برای بازگشت
            return new UserSubscriptionDto(
                subscription.Id,
                subscription.SubscriptionPlanId,
                subscription.StartDate,
                subscription.EndDate,
                subscription.TotalSessions,
                subscription.RemainingSessions,
                subscription.IsValid // وضعیت منطقی قفل
            );
        }
    }
}
