using FitnessPlatform.Domain.Billing;

namespace FitnessPlatform.Domain.Repositories
{
    public interface IUserSubscriptionRepository
    {
        Task AddAsync(UserSubscription subscription, CancellationToken cancellationToken = default);

        // ⚡️ اضافه شدن متد آپدیت برای ذخیره تغییرات جلسات و تاریخ
        Task UpdateAsync(UserSubscription subscription, CancellationToken cancellationToken = default);
        Task<UserSubscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // پیدا کردن اشتراکِ فعالِ کاربر (برای بررسی قفل هوشمند قبل از ثبت تمرین جدید)
        Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
