using FitnessPlatform.Application.Features.WorkoutSessions.Commands.CompleteSession;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkoutSessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // تزریق MediatR به جای تزریق مستقیم سرویس‌ها یا ریپازیتوری‌ها
        public WorkoutSessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // یک Endpoint از نوع PATCH یا POST برای تغییر وضعیت یک جلسه
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteSession(Guid id)
        {
            // ۱. ساخت Command با شناسه‌ای که در URL آمده است
            var command = new CompleteWorkoutSessionCommand(id);

            // ۲. ارسال Command به قلب سیستم (MediatR)
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new { Message = "تمرین با موفقیت به پایان رسید و رویدادها در پس‌زمینه در حال پردازش هستند!" });
            }

            return BadRequest("مشکلی در ثبت پایان تمرین به وجود آمد.");
        }
    }
}
