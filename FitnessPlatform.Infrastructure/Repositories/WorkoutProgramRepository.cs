using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.Persistence;

namespace FitnessPlatform.Infrastructure.Repositories
{
    public class WorkoutProgramRepository : IWorkoutProgramRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkoutProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WorkoutProgram workoutProgram, CancellationToken cancellationToken)
        {
            // اضافه کردن برنامه به Context
            await _context.WorkoutPrograms.AddAsync(workoutProgram, cancellationToken);

            // ذخیره واقعی در دیتابیس اس‌کیو‌ال سرور
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
