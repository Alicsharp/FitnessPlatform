using FitnessPlatform.Application.Interfaces;
using FitnessPlatform.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessPlatform.Infrastructure.Security
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        // تنظیمات توکن را از طریق الگوی IOptions تزریق می‌کنیم
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string Generate(User user)
        {
            // ۱. ساخت ادعاها (Claims) - اطلاعاتی که داخل توکن قفل می‌شوند
            var claims = new Claim[]
            {
            // شناسه کاربر
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            
            // ایمیل کاربر
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
            
            // نقش کاربر (که بعداً برای قفل کردن APIها با [Authorize(Roles="Trainer")] استفاده می‌شود)
            new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // ۲. ساخت کلید رمزنگاری با استفاده از کلید مخفی (SecretKey)
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            // ۳. پیکربندی و ساخت بدنه اصلی توکن
            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddHours(1), // توکن بعد از یک ساعت منقضی می‌شود
                signingCredentials);

            // ۴. تبدیل شیء توکن به یک رشته متنی استاندارد و بازگرداندن آن
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
