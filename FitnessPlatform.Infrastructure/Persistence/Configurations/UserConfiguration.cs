using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessPlatform.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // نام جدول
            builder.ToTable("Users");

            // تنظیم کلید اصلی
            builder.HasKey(u => u.Id);

            // کانفیگ جادویی برای Value Object (ایمیل)
            // دات‌نت ۸ با قابلیت HasConversion، شیء ایمیل را در دیتابیس به عنوان یک رشته متنی ذخیره می‌کند 
            // و هنگام خواندن، دوباره آن را به شیء Email تبدیل می‌کند
            builder.Property(u => u.Email)
                   .HasConversion(
                       email => email.Value,          // زمان ذخیره در دیتابیس
                       value => Email.Create(value))  // زمان خواندن از دیتابیس
                   .HasColumnName("Email")
                   .IsRequired()
                   .HasMaxLength(255);

            // ایجاد ایندکس یونیک در سطح دیتابیس تا هیچ دو کاربری نتوانند با یک ایمیل ثبت‌نام کنند
            builder.HasIndex(u => u.Email).IsUnique();

            // کانفیگ رمز عبور
            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(500);

            // کانفیگ نقش کاربر (ذخیره به صورت متن خوانا به جای عدد)
            builder.Property(u => u.Role)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(50);

            // تاریخ ثبت‌نام
            builder.Property(u => u.CreatedAt)
                   .IsRequired();
        }
    }
}
