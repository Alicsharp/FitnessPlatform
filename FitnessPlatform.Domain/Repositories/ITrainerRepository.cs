using FitnessPlatform.Domain.Entities;

namespace FitnessPlatform.Domain.Repositories
{
    public interface ITrainerRepository
    {
        Task AddAsync(Trainer trainer, CancellationToken cancellationToken = default);
        Task<Trainer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Trainer?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
