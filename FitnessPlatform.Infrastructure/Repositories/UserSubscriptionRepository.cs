using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Persistence.FitnessPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Infrastructure.Repositories
{
    public partial class UserRepository
    {
        public class UserSubscriptionRepository : IUserSubscriptionRepository
        {
            private readonly ApplicationDbContext _context;

            public UserSubscriptionRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task AddAsync(UserSubscription subscription, CancellationToken cancellationToken = default)
            {
                await _context.UserSubscriptions.AddAsync(subscription, cancellationToken);
            }

            public Task UpdateAsync(UserSubscription subscription, CancellationToken cancellationToken = default)
            {
                _context.UserSubscriptions.Update(subscription);
                return Task.CompletedTask;
            }

            public async Task<UserSubscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            {
                return await _context.UserSubscriptions
                    .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
            }

            public async Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
            {
                var now = DateTime.UtcNow;

                // ترجمه منطق IsValid به دستورات SQL توسط EF Core
                return await _context.UserSubscriptions
                    .Where(s => s.UserId == userId &&
                                s.RemainingSessions > 0 &&
                                s.EndDate >= now)
                    .OrderByDescending(s => s.EndDate) // در صورت وجود چند رکورد، جدیدترین را می‌گیریم
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}

