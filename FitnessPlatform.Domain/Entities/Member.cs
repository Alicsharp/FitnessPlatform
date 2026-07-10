namespace FitnessPlatform.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; private set; }

        // کلید طلایی معماری ما: این شناسه به کلاس User اشاره می‌کند
        public Guid UserId { get; private set; }

        public string FullName { get; private set; }
        public decimal Weight { get; private set; } // وزن
        public decimal Height { get; private set; } // قد
        public int RemainingSessions { get; private set; } // جلسات باقیمانده
        public DateTime? SubscriptionEndDate { get; private set; } // تاریخ پایان شهریه

        private Member() { }

        private Member(Guid id, Guid userId, string fullName, decimal weight, decimal height)
        {
            Id = id;
            UserId = userId;
            FullName = fullName;
            Weight = weight;
            Height = height;
            RemainingSessions = 0; // در ابتدای ثبت‌نام، جلسه‌ای ندارد
        }

        public static Member CreateProfile(Guid userId, string fullName, decimal weight, decimal height)
        {
            return new Member(Guid.NewGuid(), userId, fullName, weight, height);
        }

        // متد تجاری: شارژ حساب کاربری
        public void PaySubscription(int sessionsToAdd, int validDays)
        {
            RemainingSessions += sessionsToAdd;
            SubscriptionEndDate = DateTime.UtcNow.AddDays(validDays);
        }

        // متد تجاری: کم کردن یک جلسه بعد از هر تمرین
        public void DeductSession()
        {
            if (RemainingSessions <= 0 || SubscriptionEndDate < DateTime.UtcNow)
                throw new InvalidOperationException("اشتراک شما به پایان رسیده است.");

            RemainingSessions--;
        }
    }
}
