using FitnessPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessPlatform.Infrastructure.Persistence.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.ToTable("Trainers");
            builder.HasKey(t => t.Id);

            // اتصال به جدول Users
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(t => t.FullName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(t => t.Specialty)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.ExperienceYears)
                   .IsRequired();
        }
    }
}
