using FitnessPlatform.Domain.Booking;

namespace FitnessPlatform.Domain.Repositories
{
    public interface IGroupClassRepository
    {
        Task AddAsync(GroupClass groupClass, CancellationToken cancellationToken = default);

        Task UpdateAsync(GroupClass groupClass, CancellationToken cancellationToken = default);

        Task<GroupClass?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // دریافت لیست کلاس‌هایی که هنوز شروع نشده‌اند و ظرفیت خالی دارند
        Task<IEnumerable<GroupClass>> GetAvailableClassesAsync(CancellationToken cancellationToken = default);
    }
}
