using AutoMapper;
using AutoMapper.QueryableExtensions;

using EMS.Application.Common.Interfaces;
using EMS.Application.Common.Interfaces.Caching;
using EMS.Application.Features.Employee.Caching;
using EMS.Application.Features.Employee.DTOs;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EMS.Application.Features.Employee.Queries.GetAll;

public class GetAllEmpoyeeQuery : ICacheableRequest<IEnumerable<EmployeeDto>>
{
    public string CacheKey => EmployeeCacheKey.GetAllCacheKey;
    public MemoryCacheEntryOptions? Options => EmployeeCacheKey.MemoryCacheEntryOptions;
}

public class GetAllEmployeeQueryHandler :
     IRequestHandler<GetAllEmpoyeeQuery, IEnumerable<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllEmployeeQueryHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmpoyeeQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Employees
                     .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
        return data;
    }
}


