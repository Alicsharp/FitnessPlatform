using FitnessPlatform.Domain.Entities;

namespace FitnessPlatform.Domain.Repositories
{
    public interface IMemberRepository
    {
        Task AddAsync(Member member, CancellationToken cancellationToken = default);
        Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // پیدا کردن پروفایل ورزشکار از طریق شناسه سیستم احراز هویت
        Task<Member?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
