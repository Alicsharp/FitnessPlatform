using FitnessPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Repositories
{
    public interface IWorkoutProgramRepository
    {
        // متدی برای اضافه کردن یک برنامه تمرینی به دیتابیس
        Task AddAsync(WorkoutProgram workoutProgram, CancellationToken cancellationToken);
    }
}
