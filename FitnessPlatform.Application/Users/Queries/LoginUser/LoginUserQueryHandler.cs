using FitnessPlatform.Application.Interfaces;
using FitnessPlatform.Domain.Repositories;
using MediatR;

namespace FitnessPlatform.Application.Users.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider; // ابزار تولید توکن

        public LoginUserQueryHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            // ۱. پیدا کردن کاربر در دیتابیس از طریق ایمیل
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                throw new Exception("ایمیل یا رمز عبور اشتباه است."); // پیام‌های خطا را برای امنیت یکسان می‌دهیم
            }

            // ۲. مقایسه رمز عبور وارد شده با هشِ ذخیره شده در دیتابیس
            bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new Exception("ایمیل یا رمز عبور اشتباه است.");
            }

            // ۳. صدور بلیت ورود (توکن امنیتی)
            string token = _jwtProvider.Generate(user);

            // ۴. تقدیم توکن به کاربر
            return token;
        }
    }
}
