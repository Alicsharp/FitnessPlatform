using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessPlatform.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; protected set; }

        // اضافه شدن لیست رویدادهای دامین برای معماری رویدادمحور
        private readonly List<object> _domainEvents = new();
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        protected Entity(Guid id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
        }

        protected Entity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        protected void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddDomainEvent(object domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
