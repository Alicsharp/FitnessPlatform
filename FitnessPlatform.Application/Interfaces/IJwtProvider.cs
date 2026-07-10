using FitnessPlatform.Domain.Entities;

namespace FitnessPlatform.Application.Interfaces
{
    public interface IJwtProvider
    {
        // این متد یک کاربر می‌گیرد و یک رشته متنی (توکن) برمی‌گرداند
        string Generate(User user);
    }
}
