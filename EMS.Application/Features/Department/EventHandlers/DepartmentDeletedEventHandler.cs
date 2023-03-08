
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using Microsoft.Extensions.Logging;

namespace EMS.Application.Features.Employee.EventHandlers;

public class DepartmentDeletedEventHandler : INotificationHandler<DeletedEvent<EmployeeEntity>>
{
    private readonly ILogger<DepartmentDeletedEventHandler> _logger;

    public DepartmentDeletedEventHandler(
        ILogger<DepartmentDeletedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(DeletedEvent<EmployeeEntity> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
        return Task.CompletedTask;
    }
}
