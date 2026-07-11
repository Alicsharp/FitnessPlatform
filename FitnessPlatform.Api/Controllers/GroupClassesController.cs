using FitnessPlatform.Application.Features.Booking.Commands.CreateGroupClass;
using FitnessPlatform.Application.Features.Booking.Commands.Enroll;
using FitnessPlatform.Application.Features.Booking.Queries.GetAvailableClasses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GroupClassesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupClassesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] CreateGroupClassCommand command)
        {
            var classId = await _mediator.Send(command);
            return Ok(new { Message = "کلاس گروهی جدید با موفقیت ایجاد شد.", ClassId = classId });
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableClasses()
        {
            var query = new GetAvailableClassesQuery();
            var classes = await _mediator.Send(query);
            return Ok(classes);
        }

        [HttpPost("{id}/enroll")]
        public async Task<IActionResult> Enroll(Guid id, [FromBody] EnrollRequest request)
        {
            // شناسه کلاس از URL (id) و شناسه کاربر از Body دریافت می‌شود
            var command = new EnrollInGroupClassCommand(request.UserId, id);

            // در صورت بروز تداخل (Race Condition)، میدیاتور به طور خودکار
            // ConcurrencyException را پرتاب می‌کند که در میان‌افزار (Middleware) هندل می‌شود
            await _mediator.Send(command);

            return Ok(new { Message = "ثبت‌نام در کلاس با موفقیت انجام شد." });
        }
    }
}
