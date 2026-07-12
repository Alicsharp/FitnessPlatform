using FitnessPlatform.Domain.Booking;
using FitnessPlatform.Domain.Common;
using FitnessPlatform.Domain.Entities;
using global::FitnessPlatform.Domain.Billing;
using Microsoft.EntityFrameworkCore;
 
using System;
using System.Collections.Generic;
using System.Text;
namespace FitnessPlatform.Infrastructure.Persistence
{


    namespace FitnessPlatform.Infrastructure.Persistence
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            public DbSet<GroupClass> GroupClasses => Set<GroupClass>();



            public DbSet<WorkoutProgram> WorkoutPrograms => Set<WorkoutProgram>();
            public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();

            // جداول سیستم احراز هویت و باشگاه
            public DbSet<User> Users => Set<User>();
            public DbSet<Member> Members => Set<Member>();
            public DbSet<Trainer> Trainers => Set<Trainer>();

            // ==========================================
            // ⚡️ جداول جدید هسته مالی و اشتراک‌ها
            // ==========================================
            public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
            public DbSet<UserSubscription> UserSubscriptions => Set<UserSubscription>();
            // ⚡️ جدول جدید صندوق پستی (Outbox)
            public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // این خط تمام تنظیمات کانفیگ لایه زیرساخت (شامل کانفیگ‌های جدید مالی) را به صورت خودکار اسکن و اعمال می‌کند
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
