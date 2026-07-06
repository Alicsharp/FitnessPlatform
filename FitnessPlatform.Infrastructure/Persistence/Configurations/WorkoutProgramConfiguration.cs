using FitnessPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Infrastructure.Persistence.Configurations
{
    public class WorkoutProgramConfiguration : IEntityTypeConfiguration<WorkoutProgram>
    {
        public void Configure(EntityTypeBuilder<WorkoutProgram> builder)
        {
            // ۱. تنظیم کلید اصلی
            builder.HasKey(p => p.Id);

            // ۲. مپ کردن هوشمندانه Value Object (Duration)
            // با این کار فیلدهای StartDate و EndDate به عنوان ستون در همان جدول برنامه‌ها ذخیره می‌شوند
            builder.OwnsOne(p => p.Duration, d =>
            {
                d.Property(x => x.StartDate).HasColumnName("StartDate").IsRequired();
                d.Property(x => x.EndDate).HasColumnName("EndDate").IsRequired();
            });

            // ۳. اتصال به فیلد خصوصی پشتوانه (_sessions) برای حفظ کپسوله‌سازی DDD
            // به ای‌اف کور می‌گوییم مستقیماً از لیست خصوصی برای خواندن و نوشتن استفاده کند، نه از پراپرتی عمومی!
            builder.HasMany(p => p.Sessions)
                   .WithOne()
                   .HasForeignKey("WorkoutProgramId")
                   .OnDelete(DeleteBehavior.Cascade);

            var navigation = builder.Metadata.FindNavigation(nameof(WorkoutProgram.Sessions));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
