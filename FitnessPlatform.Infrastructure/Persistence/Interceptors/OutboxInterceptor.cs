using FitnessPlatform.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace FitnessPlatform.Infrastructure.Persistence.Interceptors
{
    public sealed class OutboxInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            // ۱. پیدا کردن تمام Entityهایی که رویداد دامین دارند
            var outboxMessages = dbContext.ChangeTracker
                .Entries<Entity>()
                .Select(x => x.Entity)
                .SelectMany(entity =>
                {
                    var events = entity.DomainEvents.ToList();
                    entity.ClearDomainEvents(); // پاک کردن رویدادها بعد از خواندن
                    return events;
                })
                .Select(domainEvent => OutboxMessage.Create(
                    type: domainEvent.GetType().AssemblyQualifiedName!, // ذخیره نوع دقیق کلاس برای MassTransit
                    content: JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
                ))
                .ToList();

            // ۲. اضافه کردن پیام‌ها به دیتابیس در همان تراکنش
            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}