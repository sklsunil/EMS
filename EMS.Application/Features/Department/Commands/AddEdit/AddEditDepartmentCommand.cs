using AutoMapper;

using EMS.Application.Common;
using EMS.Application.Common.Exceptions;
using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Common.Mappings;
using EMS.Application.Features.Department.Caching;
using EMS.Application.Features.Department.DTOs;
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using System.ComponentModel;

namespace EMS.Application.Features.Department.Commands.AddEdit;

public class AddEditDepartmentCommand : IMapFrom<DepartmentDto>, ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Name")]
    public string Name { get; set; }
    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DepartmentCacheKey.SharedExpiryTokenSource();
}

public class AddEditDepartmentCommandHandler : IRequestHandler<AddEditDepartmentCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public AddEditDepartmentCommandHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditDepartmentCommand request, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<DepartmentDto>(request);
        if (request.Id > 0)
        {
            var item = await _context.Departments.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"The Department [{request.Id}] was not found.");
            item = _mapper.Map(dto, item);
            // raise a update domain event
            item.AddDomainEvent(new UpdatedEvent<DepartmentEntity>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = _mapper.Map<DepartmentEntity>(dto);
            // raise a create domain event
            item.AddDomainEvent(new CreatedEvent<DepartmentEntity>(item));
            _context.Departments.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }

    }
}

