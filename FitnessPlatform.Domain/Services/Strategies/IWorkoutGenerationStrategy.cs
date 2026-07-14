using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Services.Strategies
{
    // ۱. قرارداد اصلی برای تمام الگوریتم‌های آینده
    public interface IWorkoutGenerationStrategy
    {
        bool IsMatch(EquipmentType equipment, int availableMinutes);
        List<WorkoutSession> GenerateSessions(int totalDays, int availableMinutes, DateTime startDate);
    }

    // ۲. الگوریتم تخصصی برای افرادی که دمبل دارند و وقتشان کم است (HIIT)
    public class HomeHiitDumbbellStrategy : IWorkoutGenerationStrategy
    {
        public bool IsMatch(EquipmentType equipment, int availableMinutes)
        {
            // این استراتژی فقط زمانی فعال می‌شود که کاربر دمبل داشته باشد و زمانش 45 دقیقه یا کمتر باشد
            return equipment == EquipmentType.Dumbbell && availableMinutes <= 45;
        }

        public List<WorkoutSession> GenerateSessions(int totalDays, int availableMinutes, DateTime startDate)
        {
            var sessions = new List<WorkoutSession>();

            for (int i = 0; i < totalDays; i++)
            {
                var currentDate = startDate.AddDays(i);

                // منطق استراحت: هر 3 روز تمرین، 1 روز استراحت مطلق (ریکاوری)
                if ((i + 1) % 4 == 0) continue;

                // الگوریتم محاسبه کالری: در تمرینات HIIT، هر دقیقه حدود 12 تا 15 کالری می‌سوزاند
                int targetCalories = availableMinutes * 14;

                string title;
                // الگوریتم توزیع فشار: روزهای زوج تمرین قدرتی با دمبل، روزهای فرد هوازی و شدو بوکسینگ
                if (i % 2 == 0)
                {
                    title = $"فول بادی با دمبل ({availableMinutes} دقیقه) - راندهای ۴۵ ثانیه کار / ۱۵ ثانیه استراحت";
                }
                else
                {
                    title = $"شدو بوکسینگ و چابکی ({availableMinutes} دقیقه) - راندهای ۳ دقیقه‌ای / ۱ دقیقه استراحت";
                }

                // استفاده از فکتوری متدی که قبلاً در WorkoutSession نوشتید
                var session = WorkoutSession.Create(title, currentDate, targetCalories);
                sessions.Add(session);
            }

            return sessions;
        }
    }
}
