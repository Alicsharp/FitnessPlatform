using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Services.Strategies
{
    public class WorkoutGeneratorDomainService
    {
        private readonly IEnumerable<IWorkoutGenerationStrategy> _strategies;

        public WorkoutGeneratorDomainService(IEnumerable<IWorkoutGenerationStrategy> strategies)
        {
            _strategies = strategies;
        }

        public List<WorkoutSession> Generate(int totalDays, int availableMinutes, EquipmentType equipment, DateTime startDate)
        {
            // پیدا کردن اولین الگوریتمی که با شرایط کاربر مطابقت دارد (پلی‌مورفیسم)
            var strategy = _strategies.FirstOrDefault(s => s.IsMatch(equipment, availableMinutes));

            if (strategy == null)
            {
                throw new InvalidOperationException("هیچ الگوریتم تمرینی برای این شرایط و تجهیزات یافت نشد.");
            }

            // اجرای الگوریتم و تولید خروجی
            return strategy.GenerateSessions(totalDays, availableMinutes, startDate);
        }
    }
}
