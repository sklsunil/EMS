using AutoMapper;

using EMS.Application.Common;
using EMS.Application.Common.Exceptions;
using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Common.Mappings;
using EMS.Application.Features.Employee.Caching;
using EMS.Application.Features.Employee.DTOs;
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using System.ComponentModel;

namespace EMS.Application.Features.Employee.Commands.AddEdit;

public class AddEditEmployeeCommand : IMapFrom<EmployeeDto>, ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Name")]
    public string Name { get; set; }
    [Description("Email")]
    public string Email { get; set; }

    [Description("DOB")]
    public DateTime DOB { get; set; }
    public int DepartmentId { get; set; }


    public string CacheKey => EmployeeCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => EmployeeCacheKey.SharedExpiryTokenSource();
}

public class AddEditEmployeeCommandHandler : IRequestHandler<AddEditEmployeeCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public AddEditEmployeeCommandHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditEmployeeCommand request, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<EmployeeDto>(request);
        if (request.Id > 0)
        {
            var item = await _context.Employees.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"The employee [{request.Id}] was not found.");
            item = _mapper.Map(dto, item);
            // raise a update domain event
            item.AddDomainEvent(new UpdatedEvent<EmployeeEntity>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            var item = _mapper.Map<EmployeeEntity>(dto);
            // raise a create domain event
            item.AddDomainEvent(new CreatedEvent<EmployeeEntity>(item));
            _context.Employees.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id);
        }

    }
}

