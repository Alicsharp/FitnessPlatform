using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Persistence.FitnessPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace FitnessPlatform.Infrastructure.Repositories
{

    public class TrainerRepository : ITrainerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TrainerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Trainer trainer, CancellationToken cancellationToken = default)
        {
            await _dbContext.Trainers.AddAsync(trainer, cancellationToken);
        }

        public async Task<Trainer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<Trainer?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Trainers.FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken);
        }
    }
}
