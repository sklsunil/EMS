using AutoMapper;

using EMS.Application.Common;
using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Features.Department.Caching;
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace EMS.Application.Features.Department.Commands.Delete;

public class DeleteDepartmentCommand : ICacheInvalidatorRequest<Result>
{
    public List<int> Id { get; }
    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DepartmentCacheKey.SharedExpiryTokenSource();
    public DeleteDepartmentCommand(List<int> id)
    {
        Id = id;
    }
}

public class DeleteDepartmentCommandHandler :
             IRequestHandler<DeleteDepartmentCommand, Result>

{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public DeleteDepartmentCommandHandler(
        IApplicationDbContext context,
         IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Departments.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            // raise a delete domain event
            item.AddDomainEvent(new DeletedEvent<DepartmentEntity>(item));
            _context.Departments.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }

}

