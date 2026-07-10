using System.Text.RegularExpressions;

namespace FitnessPlatform.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        // سازنده خصوصی برای جلوگیری از ساخت شیء نامعتبر
        private Email(string value)
        {
            Value = value;
        }

        // متد Factory برای اعتبارسنجی قبل از ساخت
        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ایمیل نمی‌تواند خالی باشد.");

            if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("فرمت ایمیل نامعتبر است.");

            return new Email(value);
        }
    }
}