using FitnessPlatform.Domain.Common;
using FitnessPlatform.Domain.ValueObjects;
using FitnessPlatform.Domain.ValueObjects.FitnessPlatform.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Entities
{
    public class WorkoutProgram : Entity
    {
        public Guid UserId { get; private set; }
        public string Title { get; private set; }
        public string Objective { get; private set; }
        public ProgramDuration Duration { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<WorkoutSession> _sessions = new();
        public IReadOnlyCollection<WorkoutSession> Sessions => _sessions.AsReadOnly();

        // رفع اخطار CS8618 با دادن مقدار پیش‌فرض در سازنده مخصوص پایگاه داده
        private WorkoutProgram()
        {
            Title = string.Empty;
            Objective = string.Empty;
            Duration = null!; // این مورد در تنظیمات EF Core پیکربندی خواهد شد
        }

        public static WorkoutProgram Create(Guid userId, string title, string objective, ProgramDuration duration)
        {
            var program = new WorkoutProgram
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = title,
                Objective = objective,
                Duration = duration,
                IsActive = true
            };

            // پرتاب رویداد بیزینسی برای RabbitMQ (در مراحل بعدی یک کلاس اختصاصی برایش می‌سازیم)
            program.AddDomainEvent(new { EventName = "WorkoutProgramCreated", UserId = userId, ProgramId = program.Id });

            return program;
        }

        public void AddSession(WorkoutSession session)
        {
            // چک می‌کنیم که کاربر در یک روز، دو جلسه تمرینی با یک نام نداشته باشد
            if (_sessions.Any(s => s.Date.Date == session.Date.Date && s.Title == session.Title))
            {
                throw new InvalidOperationException("شما قبلاً این تمرین را برای این تاریخ ثبت کرده‌اید.");
            }

            _sessions.Add(session);
            UpdateTimestamp();
        }
    }
}
