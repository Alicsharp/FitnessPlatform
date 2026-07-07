using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.ValueObjects
{
    namespace FitnessPlatform.Domain.ValueObjects
    {
        // تبدیل به رکورد برای پشتیبانی ذاتی از Value Equality در تفکر DDD
        public record ProgramDuration
        {
            public DateTime StartDate { get; init; }
            public DateTime EndDate { get; init; }

            public ProgramDuration(DateTime startDate, DateTime endDate)
            {
                if (startDate >= endDate)
                    throw new ArgumentException("تاریخ پایان باید بعد از تاریخ شروع باشد.");

                StartDate = startDate;
                EndDate = endDate;
            }

            // یک متد کاربردی برای محاسبه تعداد روزها
            public int GetTotalDays() => (EndDate - StartDate).Days;
        }
    }
}