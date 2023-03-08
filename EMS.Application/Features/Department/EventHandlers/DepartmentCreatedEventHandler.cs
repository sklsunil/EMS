

using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using Microsoft.Extensions.Logging;

namespace EMS.Application.Features.Department.EventHandlers;

public class DepartmentCreatedEventHandler : INotificationHandler<CreatedEvent<DepartmentEntity>>
{
    private readonly ILogger<DepartmentCreatedEventHandler> _logger;

    public DepartmentCreatedEventHandler(
        ILogger<DepartmentCreatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(CreatedEvent<DepartmentEntity> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
        return Task.CompletedTask;
    }
}
