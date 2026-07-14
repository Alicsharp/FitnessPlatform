using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Services.Strategies
{
    // الگوریتم تخصصی برای بدنسازی در باشگاه مجهز
    public class BodybuilderGymStrategy : IWorkoutGenerationStrategy
    {
        public bool IsMatch(EquipmentType equipment, int availableMinutes)
        {
            // این الگوریتم فقط برای باشگاه کامل و زمان بالای 60 دقیقه فعال می‌شود
            return equipment == EquipmentType.FullGym && availableMinutes >= 60;
        }

        public List<WorkoutSession> GenerateSessions(int totalDays, int availableMinutes, DateTime startDate)
        {
            var sessions = new List<WorkoutSession>();

            // سیستم تمرینی سه‌روزه (Push / Pull / Legs)
            string[] splits = {
                "سینه، سرشانه و پشت‌بازو (Push Day)",
                "زیربغل، کول و جلوبازو (Pull Day)",
                "عضلات پا و شکم (Leg Day)"
            };

            int workoutDayCounter = 0;

            for (int i = 0; i < totalDays; i++)
            {
                var currentDate = startDate.AddDays(i);

                // منطق استراحت: ۳ روز تمرین سنگین، ۱ روز استراحت مطلق برای ریکاوری عضلات
                if ((i + 1) % 4 == 0) continue;

                // تمرینات قدرتی با وزنه سنگین در لحظه کالری کمتری نسبت به هوازی می‌سوزانند (حدود ۸ کالری در دقیقه)
                int targetCalories = availableMinutes * 8;

                // انتخاب نوع تمرین بر اساس چرخه (Modulus)
                string splitName = splits[workoutDayCounter % splits.Length];
                string title = $"بدنسازی تخصصی باشگاه - {splitName} ({availableMinutes} دقیقه)";

                var session = WorkoutSession.Create(title, currentDate, targetCalories);
                sessions.Add(session);

                workoutDayCounter++; // شمارنده فقط در روزهای تمرینی جلو می‌رود
            }

            return sessions;
        }
    }
}
