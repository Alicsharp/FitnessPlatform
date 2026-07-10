using FitnessPlatform.Application.Interfaces;
using System;
using System.Collections.Generic;

namespace FitnessPlatform.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            // تولید یک هش یک‌طرفه و بسیار امن همراه با Salt خودکار
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hashedPassword)
        {
            // مقایسه رمز عبور وارد شده با هش ذخیره شده در دیتابیس
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
