using AutoMapper;

using EMS.Application.Common;
using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Features.Employee.Caching;
using EMS.Domain.Entities;
using EMS.Domain.Events;

using MediatR;

using System.ComponentModel;

namespace EMS.Application.Features.Employee.Commands.Update;

public class UpdateEmployeeCommand : ICacheInvalidatorRequest<Result>
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

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateEmployeeCommandHandler(
        IApplicationDbContext context,
         IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Employees.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item != null)
        {
            item.DepartmentId = request.DepartmentId;
            item.DOB = request.DOB;
            item.Email = request.Email;
            item.Name = request.Name;
            // raise a update domain event
            item.AddDomainEvent(new UpdatedEvent<EmployeeEntity>(item));
            await _context.SaveChangesAsync(cancellationToken);
        }
        return await Result.SuccessAsync();
    }
}

