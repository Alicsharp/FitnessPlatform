using FitnessPlatform.Domain.Common;
using FitnessPlatform.Domain.ValueObjects;
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

        // کپسوله‌سازی لیست جلسات. بیرون از کلاس نمی‌توانند مستقیماً به لیست Add کنند.
        private readonly List<WorkoutSession> _sessions = new();
        public IReadOnlyCollection<WorkoutSession> Sessions => _sessions.AsReadOnly();

        private WorkoutProgram() { } // برای EF Core

        public static WorkoutProgram Create(Guid userId, string title, string objective, ProgramDuration duration)
        {
            return new WorkoutProgram
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = title,
                Objective = objective,
                Duration = duration,
                IsActive = true
            };
        }

        // منطق تجاری: اضافه کردن جلسه تمرینی جدید با کنترل خطا
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
