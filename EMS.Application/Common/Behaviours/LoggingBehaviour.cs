using MediatR.Pipeline;

using Microsoft.Extensions.Logging;

namespace EMS.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;


    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;

    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = nameof(TRequest);
        _logger.LogTrace("Request: {Name} with {@Request}",
            requestName, request);
        return Task.CompletedTask;
    }
}
