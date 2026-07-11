using FitnessPlatform.Application.Exceptions;
using FitnessPlatform.Domain.Booking;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Persistence.FitnessPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Infrastructure.Repositories
{
    public partial class GroupClassRepository : IGroupClassRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupClassRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GroupClass groupClass, CancellationToken cancellationToken = default)
        {
            await _context.GroupClasses.AddAsync(groupClass, cancellationToken);
        }

        public async Task UpdateAsync(GroupClass groupClass, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.GroupClasses.Update(groupClass);

                // ⚡️ اینجا تنها جایی است که با دیتابیس صحبت می‌کنیم و خطا را می‌گیریم
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // ⚡️ تبدیل خطای کثیفِ دیتابیس به خطای تمیزِ لایه اپلیکیشن
                throw new ConcurrencyException("تداخل در ذخیره‌سازی داده‌ها رخ داد.", ex);
            }
        }

        public async Task<GroupClass?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.GroupClasses
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<GroupClass>> GetAvailableClassesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _context.GroupClasses
                .Where(c => c.StartTime > now)
                .ToListAsync(cancellationToken);
        }
    }
}
