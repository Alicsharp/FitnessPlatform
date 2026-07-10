using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Persistence.FitnessPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Infrastructure.Repositories
{
    public partial class UserRepository
    {
        public class SubscriptionPlanRepository : ISubscriptionPlanRepository
        {
            private readonly ApplicationDbContext _context;

            public SubscriptionPlanRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task AddAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default)
            {
                await _context.SubscriptionPlans.AddAsync(plan, cancellationToken);
            }

            public Task UpdateAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default)
            {
                _context.SubscriptionPlans.Update(plan);
                return Task.CompletedTask;
            }

            public async Task<SubscriptionPlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            {
                return await _context.SubscriptionPlans
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            }

            public async Task<IEnumerable<SubscriptionPlan>> GetActivePlansAsync(CancellationToken cancellationToken = default)
            {
                return await _context.SubscriptionPlans
                    .Where(p => p.IsActive)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}

