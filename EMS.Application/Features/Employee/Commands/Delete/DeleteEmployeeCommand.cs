using AutoMapper;

using EMS.Application.Common;
using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Features.Employee.Caching;
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace EMS.Application.Features.Employee.Commands.Delete;

public class DeleteEmployeeCommand : ICacheInvalidatorRequest<Result>
{
    public List<int> Id { get; }
    public string CacheKey => EmployeeCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => EmployeeCacheKey.SharedExpiryTokenSource();
    public DeleteEmployeeCommand(List<int> id)
    {
        Id = id;
    }
}

public class DeleteEmployeeCommandHandler :
             IRequestHandler<DeleteEmployeeCommand, Result>

{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public DeleteEmployeeCommandHandler(
        IApplicationDbContext context,
         IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Employees.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new DeletedEvent<EmployeeEntity>(item));
            _context.Employees.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

