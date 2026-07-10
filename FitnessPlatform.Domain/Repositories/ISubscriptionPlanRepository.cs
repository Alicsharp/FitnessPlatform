using FitnessPlatform.Domain.Billing;

namespace FitnessPlatform.Domain.Repositories
{
    public interface ISubscriptionPlanRepository
    {
        Task AddAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default);

        Task<SubscriptionPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        // ⚡️ این دقیقاً همان خطی است که کامپایلر دنبال آن می‌گردد
        Task UpdateAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default);
        // دریافت لیست تمام بسته‌های فعال برای نمایش به کاربر در صفحه خرید
        Task<IEnumerable<SubscriptionPlan>> GetActivePlansAsync(CancellationToken cancellationToken = default);
    }
}
