using FitnessPlatform.Application.Features.Billing.Commands.CreatePlan;
using FitnessPlatform.Application.Features.Billing.Commands.DeactivatePlan;
using FitnessPlatform.Application.Features.Billing.Commands.ExtendSubscription;
using FitnessPlatform.Application.Features.Billing.Commands.Purchase;
using FitnessPlatform.Application.Features.Billing.Queries.GetActivePlans;
using FitnessPlatform.Application.Features.Billing.Queries.GetActiveUserSubscription;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BillingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ==========================================
        // بخش مدیریت طرح‌ها (مخصوص مدیر)
        // ==========================================

        [HttpPost("plans")]
        public async Task<IActionResult> CreatePlan([FromBody] CreateSubscriptionPlanCommand command)
        {
            var planId = await _mediator.Send(command);
            return Ok(new { Message = "طرح اشتراک با موفقیت ایجاد شد.", PlanId = planId });
        }

        [HttpPatch("plans/{id}/deactivate")]
        public async Task<IActionResult> DeactivatePlan(Guid id)
        {
            var command = new DeactivatePlanCommand(id);
            await _mediator.Send(command);
            return Ok(new { Message = "طرح با موفقیت غیرفعال شد." });
        }

        // ==========================================
        // بخش ورزشکاران (خرید و مشاهده)
        // ==========================================

        [HttpGet("plans/active")]
        public async Task<IActionResult> GetActivePlans()
        {
            var query = new GetActivePlansQuery();
            var plans = await _mediator.Send(query);
            return Ok(plans);
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseSubscription([FromBody] PurchaseSubscriptionCommand command)
        {
            // در دنیای واقعی UserId از توکن JWT خوانده می‌شود، اما فعلاً از Body می‌گیریم
            var subscriptionId = await _mediator.Send(command);
            return Ok(new { Message = "خرید با موفقیت انجام شد.", SubscriptionId = subscriptionId });
        }

        [HttpGet("subscriptions/active/{userId}")]
        public async Task<IActionResult> GetActiveSubscription(Guid userId)
        {
            var query = new GetActiveUserSubscriptionQuery(userId);
            var subscription = await _mediator.Send(query);

            if (subscription == null)
                return NotFound(new { Message = "اشتراک فعالی برای این کاربر یافت نشد." });

            return Ok(subscription);
        }

        [HttpPatch("subscriptions/{id}/extend")]
        public async Task<IActionResult> ExtendSubscription(Guid id, [FromBody] int extraDays)
        {
            var command = new ExtendSubscriptionCommand(id, extraDays);
            await _mediator.Send(command);
            return Ok(new { Message = $"اشتراک با موفقیت {extraDays} روز تمدید شد." });
        }
    }
}
