using FitnessPlatform.Application.Users.Commands;
using FitnessPlatform.Application.Users.Queries.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterUserCommand(request.Email, request.Password, request.Role);
            var userId = await _mediator.Send(command);

            return Created("", new { Message = "ثبت‌نام با موفقیت انجام شد.", UserId = userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var query = new LoginUserQuery(request.Email, request.Password);
            var token = await _mediator.Send(query);

            return Ok(new { Message = "ورود موفقیت‌آمیز بود.", Token = token });
        }
    }
}
 