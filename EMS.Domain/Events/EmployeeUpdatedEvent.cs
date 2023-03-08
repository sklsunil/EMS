using EMS.Domain.Common;
using EMS.Domain.Entities;

namespace EMS.Domain.Events;


public class EmployeeUpdatedEvent : DomainEvent
{
    public EmployeeUpdatedEvent(EmployeeEntity item)
    {
        Item = item;
    }

    public EmployeeEntity Item { get; }
}

