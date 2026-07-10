using FitnessPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessPlatform.Infrastructure.Persistence.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Members");
            builder.HasKey(m => m.Id);

            // تعریف ارتباط کلید خارجی با جدول User به صورت دستی (بدون Navigation Property)
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(m => m.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // در صورت حذف کاربر، پروفایلش هم حذف شود

            builder.Property(m => m.FullName)
                   .IsRequired()
                   .HasMaxLength(150);

            // تنظیمات اعداد اعشاری برای وزن و قد (مانند: 85.5 کیلوگرم)
            builder.Property(m => m.Weight)
                   .HasColumnType("decimal(5,2)");

            builder.Property(m => m.Height)
                   .HasColumnType("decimal(5,2)");

            builder.Property(m => m.RemainingSessions)
                   .IsRequired();

            builder.Property(m => m.SubscriptionEndDate)
                   .IsRequired(false); // می‌تواند نال باشد (هنوز شهریه نداده)
        }
    }
}
