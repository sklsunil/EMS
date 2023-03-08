using AutoMapper;

using EMS.Application.Common;
using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Common.Mappings;
using EMS.Application.Features.Employee.Caching;
using EMS.Application.Features.Employee.DTOs;
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using System.ComponentModel;

namespace EMS.Application.Features.Employee.Commands.Create;

public class CreateEmployeeCommand : IMapFrom<EmployeeDto>, ICacheInvalidatorRequest<Result<int>>
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

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateEmployeeCommandHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<EmployeeDto>(request);
        var item = _mapper.Map<EmployeeEntity>(dto);
        // raise a create domain event
        item.AddDomainEvent(new CreatedEvent<EmployeeEntity>(item));
        _context.Employees.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

