using FitnessPlatform.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Booking
{
    public class GroupClass : Entity
    {
        public string Title { get; private set; }
        public DateTime StartTime { get; private set; }
        public int MaxCapacity { get; private set; }

        // کپسوله‌سازی لیست ثبت‌نام‌نام‌شدگان
        private readonly List<Guid> _enrolledMembers = new();
        public IReadOnlyCollection<Guid> EnrolledMembers => _enrolledMembers.AsReadOnly();

        // ==========================================
        // ⚡️ قفل خوش‌بینانه (Optimistic Concurrency Token)
        // ==========================================
        public Guid Version { get; private set; }

        // سازنده پرایوت برای EF Core
        private GroupClass() { }

        public static GroupClass Create(string title, DateTime startTime, int maxCapacity)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException("ظرفیت کلاس باید بیشتر از صفر باشد.");

            var groupClass = new GroupClass
            {
                Id = Guid.NewGuid(),
                Title = title,
                StartTime = startTime,
                MaxCapacity = maxCapacity,
                Version = Guid.NewGuid() // مقداردهی نسخه اولیه
            };

            return groupClass;
        }

        // ==========================================
        // رفتار اصلی: رزرو کلاس با مدیریت همزمانی
        // ==========================================
        public void EnrollMember(Guid memberId)
        {
            // ۱. بررسی قوانین بیزینسی (Invariants)
            if (_enrolledMembers.Count >= MaxCapacity)
            {
                throw new InvalidOperationException("ظرفیت این کلاس تکمیل شده است.");
            }

            if (_enrolledMembers.Contains(memberId))
            {
                throw new InvalidOperationException("شما قبلاً در این کلاس ثبت‌نام کرده‌اید.");
            }

            // ۲. ثبت‌نام کاربر
            _enrolledMembers.Add(memberId);

            // ۳. ⚡️ تغییر نسخه در هر بار آپدیت (اینجا جادوی همزمانی اتفاق می‌افتد)
            Version = Guid.NewGuid();

            UpdateTimestamp();
        }
    }
}
