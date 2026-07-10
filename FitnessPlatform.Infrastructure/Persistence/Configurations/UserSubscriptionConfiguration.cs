using FitnessPlatform.Domain.Billing;
using FitnessPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessPlatform.Infrastructure.Persistence.Configurations
{
    public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.ToTable("UserSubscriptions");
            builder.HasKey(s => s.Id);

            // ارتباط دستی با جدول کاربران
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(s => s.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ارتباط دستی با جدول طرح‌های اشتراک
            builder.HasOne<SubscriptionPlan>()
                   .WithMany()
                   .HasForeignKey(s => s.SubscriptionPlanId)
                   .OnDelete(DeleteBehavior.Restrict); // جلوگیری از حذف طرحی که قبلاً فروخته شده است

            builder.Property(s => s.AmountPaid)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(s => s.StartDate).IsRequired();
            builder.Property(s => s.EndDate).IsRequired();
            builder.Property(s => s.TotalSessions).IsRequired();
            builder.Property(s => s.RemainingSessions).IsRequired();

            // ⚡️ بسیار مهم: EF Core نباید پراپرتی‌های محاسباتی را در دیتابیس نگاشت کند
            builder.Ignore(s => s.IsValid);
        }
    }
}
