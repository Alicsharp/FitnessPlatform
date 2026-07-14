using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Domain.Services.Strategies;
using FitnessPlatform.Domain.ValueObjects.FitnessPlatform.Domain.ValueObjects;
using MediatR;

namespace FitnessPlatform.Application.Features.WorkoutPrograms.Commands.Create
{
    public class CreateWorkoutProgramCommandHandler : IRequestHandler<CreateWorkoutProgramCommand, Guid>
    {
        private readonly IWorkoutProgramRepository _repository;
        private readonly WorkoutGeneratorDomainService _generatorService; // ⚡️ اضافه شد

        public CreateWorkoutProgramCommandHandler(
            IWorkoutProgramRepository repository,
            WorkoutGeneratorDomainService generatorService)
        {
            _repository = repository;
            _generatorService = generatorService;
        }

        public async Task<Guid> Handle(CreateWorkoutProgramCommand request, CancellationToken cancellationToken)
        {
            var duration = new ProgramDuration(request.StartDate, request.EndDate);

            // ۱. ساخت موجودیت اصلی برنامه تمرینی
            var program = WorkoutProgram.Create(
                request.UserId,
                request.Title,
                request.Objective,
                duration);

            // ۲. ⚡️ استفاده از الگوریتم هوشمند برای تولید جلسات تمرینی
            var generatedSessions = _generatorService.Generate(
                totalDays: duration.GetTotalDays(), // متدی که در Value Object نوشته بودید
                availableMinutes: request.AvailableMinutesPerDay,
                equipment: request.Equipment,
                startDate: request.StartDate);

            // ۳. ⚡️ اضافه کردن جلسات تولید شده به برنامه
            foreach (var session in generatedSessions)
            {
                program.AddSession(session);
            }

            // ۴. ذخیره برنامه و تمام جلسات آن در دیتابیس با یک تراکنش
            await _repository.AddAsync(program, cancellationToken);

            return program.Id;
        }
    }
}
