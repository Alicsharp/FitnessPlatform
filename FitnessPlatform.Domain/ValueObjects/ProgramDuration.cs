using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.ValueObjects
{
    public class ProgramDuration
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public ProgramDuration(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ArgumentException("تاریخ پایان باید بعد از تاریخ شروع باشد.");

            StartDate = startDate;
            EndDate = endDate;
        }

        // یک متد کاربردی برای محاسبه تعداد ماه‌ها یا هفته‌ها
        public int GetTotalDays() => (EndDate - StartDate).Days;
    }
}
