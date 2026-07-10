namespace FitnessPlatform.Domain.Entities
{
    public class Trainer
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; } // اتصال به کلاس User
        public string FullName { get; private set; }
        public string Specialty { get; private set; } // تخصص (مثلاً: بدنسازی، کراس‌فیت)
        public int ExperienceYears { get; private set; } // سابقه کار

        // سازنده پرایوت برای EF Core
        private Trainer() { }

        private Trainer(Guid id, Guid userId, string fullName, string specialty, int experienceYears)
        {
            Id = id;
            UserId = userId;
            FullName = fullName;
            Specialty = specialty;
            ExperienceYears = experienceYears;
        }

        public static Trainer CreateProfile(Guid userId, string fullName, string specialty, int experienceYears)
        {
            return new Trainer(Guid.NewGuid(), userId, fullName, specialty, experienceYears);
        }
    }
}
