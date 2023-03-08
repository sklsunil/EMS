﻿using EMS.Domain.Common;
using EMS.Domain.Entities;

namespace EMS.Domain.Events;

public class EmployeeDeletedEvent : DomainEvent
{
    public EmployeeDeletedEvent(EmployeeEntity item)
    {
        Item = item;
    }

    public EmployeeEntity Item { get; }
}

