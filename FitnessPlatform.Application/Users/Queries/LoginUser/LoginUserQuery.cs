using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Application.Users.Queries.LoginUser
{
    // ورودی‌ها: ایمیل و پسورد / خروجی: یک رشته متنی (توکن JWT)
    public record LoginUserQuery(string Email, string Password) : IRequest<string>;
}
