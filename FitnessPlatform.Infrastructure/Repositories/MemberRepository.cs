using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Persistence.FitnessPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MemberRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Member member, CancellationToken cancellationToken = default)
        {
            await _dbContext.Members.AddAsync(member, cancellationToken);
        }

        public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<Member?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Members.FirstOrDefaultAsync(m => m.UserId == userId, cancellationToken);
        }
    }
}
