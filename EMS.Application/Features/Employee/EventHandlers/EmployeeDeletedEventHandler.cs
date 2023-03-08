
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using Microsoft.Extensions.Logging;

namespace EMS.Application.Features.Employee.EventHandlers;

public class EmployeeDeletedEventHandler : INotificationHandler<DeletedEvent<EmployeeEntity>>
{
    private readonly ILogger<EmployeeDeletedEventHandler> _logger;

    public EmployeeDeletedEventHandler(
        ILogger<EmployeeDeletedEventHandler> logger
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
