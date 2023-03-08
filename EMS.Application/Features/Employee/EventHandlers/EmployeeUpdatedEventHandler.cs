using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using Microsoft.Extensions.Logging;

namespace EMS.Application.Features.Employee.EventHandlers;

public class EmployeeUpdatedEventHandler : INotificationHandler<UpdatedEvent<EmployeeEntity>>
{
    private readonly ILogger<EmployeeUpdatedEventHandler> _logger;

    public EmployeeUpdatedEventHandler(
        ILogger<EmployeeUpdatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(UpdatedEvent<EmployeeEntity> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
        return Task.CompletedTask;
    }
}
