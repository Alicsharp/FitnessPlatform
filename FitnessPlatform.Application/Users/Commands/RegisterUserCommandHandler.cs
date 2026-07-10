using FitnessPlatform.Application.Interfaces;
using FitnessPlatform.Domain.Entities;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Domain.ValueObjects;
using MediatR;

namespace FitnessPlatform.Application.Users.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository; // ریپازیتوری فعال شد!

        public RegisterUserCommandHandler(IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // ۱. اعتبارسنجی ایمیل
            var email = Email.Create(request.Email);

            // ۲. چک کردن اینکه آیا ایمیل تکراری است یا خیر؟
            bool isUnique = await _userRepository.IsEmailUniqueAsync(email.Value, cancellationToken);
            if (!isUnique)
            {
                throw new Exception("این ایمیل قبلاً در سیستم ثبت شده است.");
                // (در پروژه‌های پیشرفته‌تر، اینجا از Custom Exceptions استفاده می‌کنیم)
            }

            // ۳. هش کردن رمز عبور
            var hashedPassword = _passwordHasher.Hash(request.Password);

            // ۴. ساخت موجودیت کاربر
            var user = User.Register(email, hashedPassword, request.Role);

            // ۵. ذخیره در دیتابیس (بدون اینکه بدانیم دیتابیس SQL است یا چیز دیگر)
            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            // ۶. برگرداندن شناسه کاربر
            return user.Id;
        }
    }
}
