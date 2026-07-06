using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Features.WorkoutPrograms.Commands.Create
{
    public record CreateWorkoutProgramCommand(
     Guid UserId,
     string Title,
     string Objective,
     DateTime StartDate,
     DateTime EndDate) : IRequest<Guid>;
    public class CreateWorkoutProgramCommandHandler : IRequestHandler<CreateWorkoutProgramCommand, Guid>
    {
        private readonly IWorkoutProgramRepository _repository;

        // تزریق وابستگی (Dependency Injection) برای گرفتن مخزن دیتابیس
        public CreateWorkoutProgramCommandHandler(IWorkoutProgramRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateWorkoutProgramCommand request, CancellationToken cancellationToken)
        {
            // ۱. ساخت شیء مقداری (Value Object) برای بررسی صحت تاریخ‌ها
            var duration = new ProgramDuration(request.StartDate, request.EndDate);

            // ۲. ساخت موجودیت اصلی با استفاده از متد امنی که در Domain نوشتیم
            var program = WorkoutProgram.Create(
                request.UserId,
                request.Title,
                request.Objective,
                duration);

            // ۳. ذخیره در دیتابیس (از طریق قرارداد/اینترفیس)
            await _repository.AddAsync(program, cancellationToken);

            // ۴. برگرداندن شناسه برنامه تمرینی ساخته شده
            return program.Id;
        }
    }
}
