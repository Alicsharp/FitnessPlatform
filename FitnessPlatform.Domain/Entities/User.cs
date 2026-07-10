using FitnessPlatform.Domain.Enums;
using FitnessPlatform.Domain.ValueObjects;

namespace FitnessPlatform.Domain.Entities
{
    public class User
    {
        // شناسه‌ها و پراپرتی‌ها فقط خواندنی (پرایوت ست) هستند
        public Guid Id { get; private set; }
        public Email Email { get; private set; }
        public string PasswordHash { get; private set; }
        public Role Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // سازنده خالی و پرایوت فقط برای استفاده EF Core (ORM)
        private User() { }

        private User(Guid id, Email email, string passwordHash, Role role)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }

        // تنها راه ساخت کاربر جدید در کل سیستم!
        public static User Register(Email email, string passwordHash, Role role)
        {
            var user = new User(Guid.NewGuid(), email, passwordHash, role);

            // در اینجا می‌توانیم یک رویداد به RabbitMQ بفرستیم: UserRegisteredEvent

            return user;
        }

        // متد اختصاصی برای تغییر رمز عبور (منطق تجاری درون کلاس)
        public void ChangePassword(string newPasswordHash)
        {
            // اینجا می‌توان قوانین چک کردن پسورد قبلی را هم گذاشت
            PasswordHash = newPasswordHash;
        }
    }
}
