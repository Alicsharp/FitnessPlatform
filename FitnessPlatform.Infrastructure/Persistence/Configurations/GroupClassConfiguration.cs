using FitnessPlatform.Domain.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessPlatform.Infrastructure.Persistence.Configurations
{
    public class GroupClassConfiguration : IEntityTypeConfiguration<GroupClass>
    {
        public void Configure(EntityTypeBuilder<GroupClass> builder)
        {
            builder.ToTable("GroupClasses");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Title).IsRequired().HasMaxLength(150);
            builder.Property(g => g.StartTime).IsRequired();
            builder.Property(g => g.MaxCapacity).IsRequired();

            // ⚡️ تعریف فیلد Version به عنوان Concurrency Token
            builder.Property(g => g.Version)
                   .IsConcurrencyToken();

            // نکته: لیست EnrolledMembers در یک جدول جداگانه (مثلاً GroupClassMembers) با رابطه Many-to-Many نقشه‌برداری می‌شود
            // که می‌توانیم آن را با متد HasMany و WithMany پیکربندی کنیم.
        }
    }
}
