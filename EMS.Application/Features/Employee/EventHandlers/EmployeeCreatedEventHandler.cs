

using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using Microsoft.Extensions.Logging;

namespace EMS.Application.Features.Employee.EventHandlers;

public class EmployeeCreatedEventHandler : INotificationHandler<CreatedEvent<EmployeeEntity>>
{
    private readonly ILogger<EmployeeCreatedEventHandler> _logger;

    public EmployeeCreatedEventHandler(
        ILogger<EmployeeCreatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(CreatedEvent<EmployeeEntity> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
        return Task.CompletedTask;
    }
}
