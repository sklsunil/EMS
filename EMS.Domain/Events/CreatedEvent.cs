using EMS.Domain.Common;

namespace EMS.Domain.Events;
public class CreatedEvent<T> : DomainEvent where T : BaseEntity
{
    public CreatedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
