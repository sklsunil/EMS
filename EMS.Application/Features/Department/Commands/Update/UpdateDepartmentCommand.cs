using AutoMapper;

using EMS.Application.Common;
using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Features.Department.Caching;
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using System.ComponentModel;

namespace EMS.Application.Features.Department.Commands.Update;

public class UpdateDepartmentCommand : ICacheInvalidatorRequest<Result>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; }

    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DepartmentCacheKey.SharedExpiryTokenSource();
}

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateDepartmentCommandHandler(
        IApplicationDbContext context,
         IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Departments.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item != null)
        {
            item.Name = request.Name;
            item.LastModified = DateTime.Now;
            // raise a update domain event
            item.AddDomainEvent(new UpdatedEvent<DepartmentEntity>(item));
            await _context.SaveChangesAsync(cancellationToken);
        }
        return await Result.SuccessAsync();
    }
}

