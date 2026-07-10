using FitnessPlatform.Domain.Entities;

namespace FitnessPlatform.Domain.Repositories
{
    public interface IUserRepository
    {
        // اضافه کردن کاربر جدید به دیتابیس
        Task AddAsync(User user, CancellationToken cancellationToken = default);

        // پیدا کردن کاربر با استفاده از ایمیل (برای زمان لاگین کردن)
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        // چک کردن اینکه آیا این ایمیل قبلاً ثبت‌نام کرده است یا خیر؟
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);

        // ذخیره تغییرات
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
