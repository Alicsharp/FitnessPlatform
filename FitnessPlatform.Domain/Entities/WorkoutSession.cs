using FitnessPlatform.Domain.Common;

namespace FitnessPlatform.Domain.Entities
{
    public class WorkoutSession : Entity
    {
        public string Title { get; private set; }
        public DateTime Date { get; private set; }
        public int TargetCaloriesBurn { get; private set; }
        public bool IsCompleted { get; private set; }

        // مقداردهی اولیه برای رفع اخطارها
        private WorkoutSession()
        {
            Title = string.Empty;
        }

        // کلمه internal باعث می‌شود این متد فقط در لایه Domain قابل استفاده باشد
        // در نتیجه تنها کلاس WorkoutProgram اجازه ساخت Session را خواهد داشت!
        public static WorkoutSession Create(string title, DateTime date, int targetCaloriesBurn)
        {
            return new WorkoutSession
            {
                Id = Guid.NewGuid(),
                Title = title,
                Date = date,
                TargetCaloriesBurn = targetCaloriesBurn,
                IsCompleted = false
            };
        }

        // منطق تجاری: تکمیل کردن جلسه تمرین
        public void MarkAsCompleted()
        {
            if (IsCompleted)
                throw new InvalidOperationException("این جلسه قبلاً تکمیل شده است.");

            IsCompleted = true;
            UpdateTimestamp();
        }
    }
}
