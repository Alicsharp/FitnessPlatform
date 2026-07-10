using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Persistence.FitnessPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Infrastructure.Repositories
{
    public partial class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            // استفاده از AsNoTracking برای سرعت بیشتر در زمان خواندن لاگین
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            // بررسی سریع دیتابیس برای جلوگیری از ثبت‌نام تکراری
            return !await _dbContext.Users.AnyAsync(u => u.Email.Value == email, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
